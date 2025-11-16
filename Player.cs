using Godot;


public partial class Player : CharacterBody2D
{
    // How fast the player moves
    [Export] public float Speed = 300.0f;
    [Export] public Texture2D FrontSprite;
    [Export] public Texture2D BackSprite;

    // NEW: Weapon system
    [Export] public PackedScene ProjectileScene; // Assign in Inspector!
    [Export] public float AttackCooldown = 1.0f; // Shoot every 1 second

    private Sprite2D _sprite;
    private float _attackTimer = 1.0f; //Track time until next shot

    public override void _Ready()
    {
        // Cache the sprite reference
        _sprite = GetNode<Sprite2D>("Sprite2D");
    }


    // This runs every physics frame (60 times per second)
    public override void _PhysicsProcess(double delta)
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
        else
        {
            Velocity = Vector2.Zero; // Stop when no input
        }

        // Actually move the character (Godot handles collision automatically)
        MoveAndSlide();
    }


    // NEW: Weapon system
    public override void _Process(double delta)
    {
        // Count down the attack timer
        _attackTimer -= (float)delta;

        // Time to shoot?
        if (_attackTimer <= 0f)
        {
            ShootAtNearestEnemy();
            _attackTimer = AttackCooldown; // Reset timer
        }
    }

    // NEW: Find and shoot at nearest enemy
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

    // NEW: Actually create and fire the projectile
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