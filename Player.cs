using Godot;


public partial class Player : CharacterBody2D
{
    // How fast the player moves
    [Export] public float Speed = 300.0f;
    [Export] public Texture2D FrontSprite;
    [Export] public Texture2D BackSprite;

    private Sprite2D _sprite;

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

    // TEMPORARY - Just for testing
    public override void _Process(double delta)
    {
        // Press SPACE to damage nearby enemies (test only)
        if (Input.IsActionJustPressed("ui_accept")) // Space bar
        {
            TestDamageNearbyEnemies();
        }
    }

// TEMPORARY - Delete this later
    private void TestDamageNearbyEnemies()
    {
        // Find all enemies in the scene
        var enemies = GetTree().GetNodesInGroup("enemies");

        foreach (var node in enemies)
        {
            if (node is Enemy enemy)
            {
                enemy.TakeDamage(3); // Test: do 3 damage
            }
        }
    }
}