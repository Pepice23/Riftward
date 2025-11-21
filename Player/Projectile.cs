using Godot;

#pragma warning disable CA1050
public partial class Projectile : Area2D
#pragma warning restore CA1050
{
    // How fast the projectile moves
    [Export] public float Speed = 400.0f;

    // How much damage it deals
    [Export] public int Damage = 3;

    // Which direction it's flying
    private Vector2 _direction;

    private Sprite2D _sprite;

    // Call this when spawning the projectile to set its direction
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.Normalized();
    }

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("%Sprite2D");
        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Mage)
        {
            _sprite.Texture = GD.Load<Texture2D>("res://Assets/Sprites/projectiles/mage_bolt.png");
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Hunter)
        {
            _sprite.Texture = GD.Load<Texture2D>("res://Assets/Sprites/projectiles/hunter_arrow.png");
        }

        // When projectile hits something, call this function
        BodyEntered += OnBodyEntered;

        // Auto-destroy after 3 seconds (so it doesn't exist forever)
        GetTree().CreateTimer(3.0).Timeout += QueueFree;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Move the projectile forward
        Position += _direction * Speed * (float)delta;
    }

    private void OnBodyEntered(Node2D body)
    {
        // If we hit an enemy, damage it
        if (body is Enemy enemy)
        {
            enemy.TakeDamage(Damage);
            QueueFree(); // Destroy the projectile
        }

        if (body is EliteEnemy eliteEnemy)
        {
            eliteEnemy.TakeDamage(Damage);
            QueueFree(); // Destroy the projectile
        }
    }
}