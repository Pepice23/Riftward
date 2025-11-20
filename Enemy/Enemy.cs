using System.Collections.Generic;
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

    private Sprite2D _sprite;

    private readonly List<string> _enemySpritePaths =
    [
        "res://Assets/Sprites/enemies/swarm/bandit.png", "res://Assets/Sprites/enemies/swarm/gnoll.png",
        "res://Assets/Sprites/enemies/swarm/kobold.png"
    ];

    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("%EnemySprite");
        UpdateSprite();
        // Find the player in the scene
        // We'll look for any node named "Player"
        _player = GetTree().Root.FindChild("Player", true, false) as Player;
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        // Initialize health
        _currentHealth = MaxHealth;
        UpdateEnemyHPBar();
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
        UpdateEnemyHPBar();

        GD.Print($"Kobold took {amount} damage! Health: {_currentHealth}/{MaxHealth}");

        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GD.Print("Kobold died!");
        _player?.GainXP(5);
        QueueFree(); // Remove from scene
    }

    private void UpdateEnemyHPBar()
    {
        if (_healthBar != null)
        {
            _healthBar.MaxValue = MaxHealth;
            _healthBar.Value = _currentHealth;
        }
    }


    private void UpdateSprite()
    {
        //Get random sprite from list
        var randomNumber = GD.RandRange(0, _enemySpritePaths.Count - 1);
        var path = _enemySpritePaths[randomNumber];
        _sprite.Texture = GD.Load<Texture2D>(path);
    }
}