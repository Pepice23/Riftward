using Godot;

public partial class Enemy : CharacterBody2D
{
    // How fast the enemy moves
    [Export] public float Speed = 100.0f;
    [Export] public int MaxHealth = 10;

    private int _currentHealth;

    // Reference to the player (we'll find this automatically)
    private Player _player;

    // Reference to the progressbar
    private ProgressBar _healthBar;

    public override void _Ready()
    {
        // Find the player in the scene
        // We'll look for any node named "Player"
        _player = GetTree().Root.FindChild("Player", true, false) as Player;
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdateEnemyHP();
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

    // This method will be called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        UpdateEnemyHP();

        GD.Print($"Kobold took {amount} damage! Health: {_currentHealth}/{MaxHealth}");

        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GD.Print("Kobold died!");
        _player?.GainXP(1);
        QueueFree(); // Remove from scene
    }

    private void UpdateEnemyHP()
    {
        _healthBar.MaxValue = MaxHealth;
        _healthBar.Value = _currentHealth;
    }
}