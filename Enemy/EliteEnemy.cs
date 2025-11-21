using System.Collections.Generic;
using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class EliteEnemy : CharacterBody2D
#pragma warning restore CA1050
{
    // How fast the enemy moves
    [Export] public float Speed = 70.0f;
    [Export] public int MaxHealth = 40;
    [Export] public int StopDistance = 50;

    [Export] public Texture2D EnemySprite;

    private int _currentHealth;

    // Reference to the player (we'll find this automatically)
    private Player _player;

    // Reference to the progressbar
    private ProgressBar _healthBar;

    private Sprite2D _sprite;

    private readonly List<string> _enemySpritePaths =
    [
        "res://Assets/Sprites/enemies/elite/death_knight.png",
        "res://Assets/Sprites/enemies/elite/corrupted_paladin.png",
        "res://Assets/Sprites/enemies/elite/fel_warlock.png", "res://Assets/Sprites/enemies/elite/beastmaster.png"
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

        // Calculate the distance to the player
        var distance = GlobalPosition.DistanceTo(_player.GlobalPosition);

        // Move toward the player
        if (distance > StopDistance)
        {
            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }

    // This method will be called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        UpdateEnemyHPBar();

        GD.Print($"Elite enemy took {amount} damage! Health: {_currentHealth}/{MaxHealth}");

        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        GD.Print("Elite enemy died!");
        _player?.GainXP(25);
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
        EnemySprite = GD.Load<Texture2D>(path);
        _sprite.Texture = EnemySprite;
    }
}