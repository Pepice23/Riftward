using System.Threading.Tasks;
using Godot;


public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void XPUpdatedEventHandler(int xpNeededForNextLevel, int currentXP);

    #region Exports

    // Movement
    [Export] public float Speed = 300.0f;
    [Export] public Texture2D FrontSprite;
    [Export] public Texture2D BackSprite;

    // Combat
    [Export] public PackedScene ProjectileScene; // Assign in Inspector!
    [Export] public float AttackCooldown = 1.0f; // Shoot every 1 second

    // Health
    [Export] public int MaxHealth = 100;
    [Export] public float DamageFlashDuration = 0.1f; // How long to flash red when hit

    // XP
    [Export] public int CurrentLevel = 1;
    [Export] public int CurrentXP;
    [Export] public int BaseXPNeeded = 10; // XP needed for level 2
    [Export] public CanvasLayer LevelUpUi;
    [Export] public CanvasLayer Hud;

    #endregion

    #region Private Fields

    // References
    private ProgressBar _healthBar;
    private Sprite2D _sprite;

    // Combat state
    private float _attackTimer = 1.0f; //Track time until next shot

    // Health State
    private int _currentHealth;
    private float _damageCooldown;
    private const float DamageCooldownTime = 0.5f;
    private bool _isDead;

    // XP
    private int _xpNeededForNextLevel;


    // Upgrade Buttons
    private Button _upgrade1;
    private Button _upgrade2;
    private Button _upgrade3;

    // Character Changing Buttons

    #endregion


    #region Lifecycle Methods

    public override void _Ready()
    {
        var hud = Hud as Hud;

        // Cache the sprite reference
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        _upgrade1 = LevelUpUi.GetNode<Button>("Panel/VBoxContainer/UpgradeButton1");
        _upgrade2 = LevelUpUi.GetNode<Button>("Panel/VBoxContainer/UpgradeButton2");
        _upgrade3 = LevelUpUi.GetNode<Button>("Panel/VBoxContainer/UpgradeButton3");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdatePlayerHP();
        // Initialize XP
        _xpNeededForNextLevel = CalculateXPNeeded();
        // Connect upgrade buttons
        _upgrade1.Pressed += OnUpgrade1Selected;
        _upgrade2.Pressed += OnUpgrade2Selected;
        _upgrade3.Pressed += OnUpgrade3Selected;

        if (hud != null)
        {
            hud.PaladinSelected += ChangeToPaladin;
            hud.MageSelected += ChangeToMage;
            hud.HunterSelected += ChangeToHunter;
        }
    }

    // This runs every physics frame (60 times per second)
    public override void _PhysicsProcess(double delta)
    {
        // NEW: Don't move if dead
        if (_isDead)
            return;

        HandleMovement(delta);
        // Continuously check for collisions with enemies
        CheckEnemyCollisions();
    }

    public override void _Process(double delta)
    {
        // NEW: Don't attack if dead
        if (_isDead)
            return;

        UpdateCooldowns(delta);

        HandleAttacking();
    }

    #endregion

    #region Movement

    private void HandleMovement(double delta)
    {
        // Get input direction (WASD or arrow keys)
        // Returns Vector2 like (-1, 0) for left, (1, 0) for right, etc.
        var direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        // Set velocity based on direction and speed
        // Normalized() makes diagonal movement same speed as straight
        if (direction != Vector2.Zero)
        {
            // Move the character
            Velocity = direction.Normalized() * Speed;
            UpdateSpriteDirection(direction);
        }

        else
        {
            Velocity = Vector2.Zero; // Stop when no input
        }

        // Actually move the character (Godot handles collision automatically)
        MoveAndSlide();
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        // Change Sprite based on vertical movement
        if (direction.Y < -0.1f) // Moving up (away from camera)
            _sprite.Texture = BackSprite;
        else // Moving down, left, right, or diagonal
            _sprite.Texture = FrontSprite;

        // Flip sprite based on horizontal movement
        if (direction.X > 0.01f)
            _sprite.FlipH = false; // Moving right
        else if (direction.X < -0.01f) _sprite.FlipH = true; // Moving left
        // If only moving up/down, keep current flip state
    }

    #endregion

    #region Combat System

    private void UpdateCooldowns(double delta)
    {
        //  Count down damage cooldown
        if (_damageCooldown > 0f) _damageCooldown -= (float)delta;

        // Count down the attack timer
        _attackTimer -= (float)delta;
    }

    private void HandleAttacking()
    {
        // Time to shoot?
        if (_attackTimer <= 0f)
        {
            ShootAtNearestEnemy();
            _attackTimer = AttackCooldown; // Reset timer
        }
    }

    //  Find and shoot at nearest enemy
    private void ShootAtNearestEnemy()
    {
        // Make sure we have a projectile scene assigned
        if (ProjectileScene == null)
            return;

        // Find all enemies
        var enemies = GetTree().GetNodesInGroup("enemies");
        if (enemies.Count == 0)
            return; // No enemies to shoot

        // Find the closest enemy
        Enemy nearestEnemy = null;
        var nearestDistance = float.MaxValue;

        foreach (var node in enemies)
            if (node is Enemy enemy)
            {
                var distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

        // Shoot at the nearest enemy
        if (nearestEnemy != null) SpawnProjectile(nearestEnemy.GlobalPosition);
    }

//  Actually create and fire the projectile
    private void SpawnProjectile(Vector2 targetPosition)
    {
        // Create the projectile
        var projectile = ProjectileScene.Instantiate<Projectile>();

        // Spawn it at player's position
        projectile.GlobalPosition = GlobalPosition;

        // Aim it at the target
        var direction = (targetPosition - GlobalPosition).Normalized();
        projectile.SetDirection(direction);

        // Add it to the scene (as child of main scene, not player!)
        GetParent().AddChild(projectile);
    }

    #endregion

    #region Health & Damage

    //  Separate method to keep things organized
    private void CheckEnemyCollisions()
    {
        // Can't take damage yet? Skip checking
        if (_damageCooldown > 0f)
            return;

        // Loop through everything we just collided with
        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            var collider = collision.GetCollider();

            // Is it an enemy?
            if (collider is Enemy enemy)
            {
                TakeDamage(10);
                return; // Stop checking - we already got hurt this frame
            }
        }
    }

    // Take damage method
    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _damageCooldown = DamageCooldownTime; // Start invulnerability

        _ = FlashDamage(); // Start the flash dont wait
        UpdatePlayerHP(); // Update immediately

        // Did we die?
        if (_currentHealth <= 0) Die();
    }

    // Death method
    private void Die()
    {
        if (_isDead) return; // Already dead dont die twice

        _isDead = true;
        GD.Print("Player died!");

        // Stop moving
        Velocity = Vector2.Zero;
    }

    private void UpdatePlayerHP()
    {
        _healthBar.MaxValue = MaxHealth;
        _healthBar.Value = _currentHealth;
    }

    private async Task FlashDamage()
    {
        // Turn red
        _sprite.Modulate = new Color(1, 0, 0); // Pure red (R=1, G=0, B=0)

        // Wait 0.1 seconds
        await ToSignal(GetTree().CreateTimer(DamageFlashDuration), SceneTreeTimer.SignalName.Timeout);

        // Return to normal (white)
        _sprite.Modulate = new Color(1, 1, 1); // White (R=1, G=1, B=1)
    }

    #endregion

    #region Character Switching

    private void ChangeToPaladin()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void ChangeToMage()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void ChangeToHunter()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_back.png");
        _sprite.Texture = FrontSprite;
    }

    #endregion

    #region XP & Level Up

    // Calculate how much XP is needed for the next level
    private int CalculateXPNeeded()
    {
        // Formula: Base * (1.15 ^ (Level - 1))
        // Level 2: 10 * 1.15^0 = 10
        // Level 3: 10 * 1.15^1 = 11.5 → 12
        // Level 4: 10 * 1.15^2 = 13.2 → 14
        return Mathf.CeilToInt(BaseXPNeeded * Mathf.Pow(1.15f, CurrentLevel - 1));
    }

    // Call this when player should gain XP
    public void GainXP(int amount)
    {
        CurrentXP += amount;
        GD.Print($"Gained {amount} XP! Total: {CurrentXP}/{_xpNeededForNextLevel}");

        // Did we level up?
        if (CurrentXP >= _xpNeededForNextLevel) LevelUp();

        EmitSignal(SignalName.XPUpdated, _xpNeededForNextLevel, CurrentXP);
    }

    private void LevelUp()
    {
        // Subtract the XP cost (carry over extra XP)
        CurrentXP -= _xpNeededForNextLevel;

        // Increase level
        CurrentLevel++;

        // Recalculate XP needed for next level
        _xpNeededForNextLevel = CalculateXPNeeded();
        EmitSignal(SignalName.XPUpdated, _xpNeededForNextLevel, CurrentXP);

        GD.Print($"LEVEL UP! Now level {CurrentLevel}. Need {_xpNeededForNextLevel} XP for next level.");

        PauseGame();
        // TODO: Show upgrade UI (we'll do this next)
    }

    #endregion

    #region Pause and Resume

    private void PauseGame()
    {
        GetTree().Paused = true;
        LevelUpUi.Visible = true;
    }

    private void ResumeGame()
    {
        GetTree().Paused = false;
    }

    #endregion

    #region Upgrade Buttons

    private void OnUpgrade1Selected()
    {
        GD.Print("Player chose Upgrade 1");
        LevelUpUi.Visible = false;
        ResumeGame();
    }

    private void OnUpgrade2Selected()
    {
        GD.Print("Player chose Upgrade 2");
        LevelUpUi.Visible = false;
        ResumeGame();
    }

    private void OnUpgrade3Selected()
    {
        GD.Print("Player chose Upgrade 3");
        LevelUpUi.Visible = false;
        ResumeGame();
    }

    #endregion
}