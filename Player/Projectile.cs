using Godot;

public partial class Projectile : Area2D
{
    // How fast the projectile moves
    [Export] public float Speed = 400.0f;

    // How much damage it deals
    [Export] public int Damage = 3;

    // Which direction it's flying
    private Vector2 _direction;

    // Call this when spawning the projectile to set its direction
    public void SetDirection(Vector2 direction)
    {
        _direction = direction.Normalized();
    }

    public override void _Ready()
    {
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
    }
}