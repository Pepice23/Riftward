using Godot;

public partial class Enemy : CharacterBody2D
{
    // How fast the enemy moves
    [Export] public float Speed = 100.0f;

    // Reference to the player (we'll find this automatically)
    private Node2D _player;

    public override void _Ready()
    {
        // Find the player in the scene
        // We'll look for any node named "Player"
        _player = GetTree().Root.FindChild("Player", true, false) as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        // If we can't find the player, don't do anything
        if (_player == null)
            return;

        // Calculate direction TO the player
        var direction = (_player.GlobalPosition - GlobalPosition).Normalized();

        // Move toward the player
        Velocity = direction * Speed;
        MoveAndSlide();
    }
}