using System.Threading.Tasks;
using Godot;


public partial class Player : CharacterBody2D
{
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

    #endregion


    #region Lifecycle Methods

    public override void _Ready()
    {
        // Cache the sprite reference
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdatePlayerHP();
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


// Weapon system
    public override void _Process(double delta)
    {
        // NEW: Don't attack if dead
        if (_isDead)
            return;

        //  Count down damage cooldown
        if (_damageCooldown > 0f)
        {
            _damageCooldown -= (float)delta;
        }

        // Count down the attack timer
        _attackTimer -= (float)delta;

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
        if (_currentHealth <= 0)
        {
            Die();
        }
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


    private void OnPaladinButtonPressed()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/paladin/paladin_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void OnMageButtonPressed()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/mage/mage_back.png");
        _sprite.Texture = FrontSprite;
    }

    private void OnHunterButtonPressed()
    {
        FrontSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_front.png");
        BackSprite = GD.Load<Texture2D>("res://Assets/Sprites/hunter/hunter_back.png");
        _sprite.Texture = FrontSprite;
    }
}