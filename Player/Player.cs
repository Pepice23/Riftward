using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;


public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void XPUpdatedEventHandler(int xpNeededForNextLevel, int currentXP);

    [Signal]
    public delegate void LevelUpdatedEventHandler(int currentLevel);

    [Signal]
    public delegate void GamePausedEventHandler();


    #region Exports

    // Movement
    [Export] public float Speed = 300.0f;
    [Export] public Texture2D FrontSprite;
    [Export] public Texture2D BackSprite;

    // Combat
    [Export] public PackedScene ProjectileScene; // Assign in Inspector!
    [Export] public float AttackCooldown = 1.0f; // Shoot every 1 second
    [Export] public float AuraDamageCooldown = 0.5f;
    [Export] public int Damage = 3;
    [Export] public int ProjectileSpeed = 400;
    [Export] public int ProjectileCount = 1;


    // Health
    [Export] public int MaxHealth = 100;
    [Export] public float DamageFlashDuration = 0.1f; // How long to flash red when hit
    [Export] public float HealthRegen;

    // XP
    [Export] public int CurrentLevel = 1;
    [Export] public int CurrentXP;
    [Export] public int BaseXPNeeded = 10; // XP needed for level 2
    [Export] public LevelUpUi LevelUpUi;
    [Export] public Hud Hud;

    // Paladin Aura
    [Export] public float HammerRotationSpeed = 2.0f; // Rotations per second
    [Export] public int AuraRadius = 100;

    #endregion

    #region Private Fields

    // References
    private ProgressBar _healthBar;
    private Sprite2D _sprite;
    private Area2D _area;
    private Node2D _hammerAura;

    // Combat state
    private float _attackTimer = 1.0f; //Track time until next shot
    private float _auraDamageTimer = 0.5f;

    // Health State
    private int _currentHealth;
    private float _damageCooldown;
    private const float DamageCooldownTime = 0.5f;
    private bool _isDead;

    // XP
    private int _xpNeededForNextLevel;

    private List<CharacterBody2D> _enemiesInAura = [];
    private CollisionShape2D _collisionShape;

    #endregion


    #region Lifecycle Methods

    public override void _Ready()
    {
        // Cache the sprite reference
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        _area = GetNode<Area2D>("Area2D");
        _hammerAura = GetNode<Node2D>("HammerAura");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdatePlayerHP();
        // Initialize XP
        CalculateXPNeeded();

        switch (GameManager.Instance.SelectedClass)
        {
            case GameManager.PlayerClass.Paladin:
                SetupPaladin();
                break;
            case GameManager.PlayerClass.Mage:
                SetupMage();
                break;
            case GameManager.PlayerClass.Hunter:
                SetupHunter();
                break;
            default:
                SetupPaladin();
                break;
        }


        GameManager.Instance.StartRun();
        _area.BodyEntered += AddEnemiesToAura;
        _area.BodyExited += RemoveEnemiesFromAura;
    }

    private void AddEnemiesToAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Add(character);
            GD.Print("Enemies entered the area");
        }
    }

    private void RemoveEnemiesFromAura(Node2D body)
    {
        if (body is CharacterBody2D character and (Enemy or EliteEnemy))
        {
            _enemiesInAura.Remove(character);
            GD.Print("Enemies exited the area");
        }
    }

    // This runs every physics frame (60 times per second)
    public override void _PhysicsProcess(double delta)
    {
        // NEW: Don't move if dead
        if (_isDead)
            return;

        HandleMovement(delta);
        // Clamping player to the scene
        Position = new Vector2(
            Mathf.Clamp(Position.X, 45, GetViewportRect().Size.X - 45),
            Mathf.Clamp(Position.Y, 45, GetViewportRect().Size.Y - 45)
        );
        // Continuously check for collisions with enemies
        CheckEnemyCollisions();
    }

    public override void _Process(double delta)
    {
        // NEW: Don't attack if dead
        if (_isDead)
            return;

        UpdateCooldowns(delta);
        // Only shoot projectiles if NOT Paladin
        if (GameManager.Instance.SelectedClass != GameManager.PlayerClass.Paladin)
            HandleAttacking(); // Your projectile shooting

        // Only do aura damage if IS Paladin
        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Paladin)
        {
            if (_auraDamageTimer <= 0f)
            {
                DamageAuraEnemies();
                _auraDamageTimer = AuraDamageCooldown;
            }


            // Show hammer aura for Paladin
            if (_hammerAura != null)
            {
                _hammerAura.Visible = true;
            }
            else
            {
                // Hide hammer for non-Paladin
                if (_hammerAura != null)
                    _hammerAura.Visible = false;
            }

            // Rotate hammer aura
            if (_hammerAura != null) _hammerAura.Rotation += HammerRotationSpeed * Mathf.Tau * (float)delta;
        }
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
        _auraDamageTimer -= (float)delta;
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
        CharacterBody2D nearestTarget = null;
        var nearestDistance = float.MaxValue;

        foreach (var node in enemies)
        {
            if (node is Enemy enemy)
            {
                var distance = GlobalPosition.DistanceTo(enemy.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = enemy;
                }
            }

            if (node is EliteEnemy eliteEnemy)
            {
                var distance = GlobalPosition.DistanceTo(eliteEnemy.GlobalPosition);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = eliteEnemy;
                }
            }
        }


        // Shoot at the nearest enemy
        if (nearestTarget != null) SpawnProjectile(nearestTarget.GlobalPosition);
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

        // Set the projectile Damage and Speed
        projectile.Damage = Damage;
        projectile.Speed = ProjectileSpeed;

        // Add it to the scene (as child of main scene, not player!)
        GetParent().AddChild(projectile);
    }

    private void DamageAuraEnemies()
    {
        foreach (var enemy in _enemiesInAura)
            if (enemy is Enemy regularEnemy)
                regularEnemy.TakeDamage(Damage);
            else if (enemy is EliteEnemy eliteEnemy) eliteEnemy.TakeDamage(Damage);
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

            // Is it an elite enemy?
            if (collider is EliteEnemy eliteEnemy)
            {
                TakeDamage(20);
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
        GameManager.Instance.EndRun(false);
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


    #region XP & Level Up

    // Calculate how much XP is needed for the next level
    private void CalculateXPNeeded()
    {
        // Formula: Base * (1.15 ^ (Level - 1))
        // Level 2: 10 * 1.15^0 = 10
        // Level 3: 10 * 1.15^1 = 11.5 → 12
        // Level 4: 10 * 1.15^2 = 13.2 → 14
        _xpNeededForNextLevel = Mathf.CeilToInt(BaseXPNeeded * Mathf.Pow(1.15f, CurrentLevel - 1));
        EmitSignal(SignalName.XPUpdated, _xpNeededForNextLevel, CurrentXP);
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
        CalculateXPNeeded();
        EmitSignal(SignalName.LevelUpdated, CurrentLevel);


        PauseGame();
    }

    #endregion

    private void SetupPaladin()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_back.png");
        _sprite.Texture = FrontSprite;

        UpdateHammerPositions(AuraRadius);
    }

    private void SetupMage()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void SetupHunter()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void PauseGame()
    {
        EmitSignal(SignalName.GamePaused);
    }

    private void UpdateHammerPositions(float radius)
    {
        var hammers = _hammerAura.GetChildren();
        var hammerCount = hammers.Count;

        for (var i = 0; i < hammerCount; i++)
        {
            // Calculate angle for this hammer (evenly distributed)
            var angle = i * (Mathf.Tau / hammerCount);

            // Calculate position on circle
            var x = radius * Mathf.Cos(angle);
            var y = radius * Mathf.Sin(angle);

            // Set the hammer's position
            if (hammers[i] is Node2D hammer)
            {
                hammer.Position = new Vector2(x, y);
            }
        }
    }
}