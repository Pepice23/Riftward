using Godot;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class BaseEnemy : CharacterBody2D
#pragma warning restore CA1050
{
    [Export] public float Speed;
    [Export] public int MaxHealth;
    [Export] public int Damage;

    protected int CurrentHealth;
    private Player _player;
    private Sprite2D _sprite;
    private ProgressBar _healthBar;


    public override void _Ready()
    {
        _sprite = GetNode<Sprite2D>("%EnemySprite");
        // Find the player in the scene
        // We'll look for any node named "Player"
        _player = GetTree().Root.FindChild("Player", true, false) as Player;
        _healthBar = GetNode<ProgressBar>("%ProgressBar");
        // Initialize health
        CurrentHealth = MaxHealth;
        UpdateEnemyHPBar();
    }

    public override void _PhysicsProcess(double delta)
    {
        // If we can't find the player, don't do anything
        if (_player == null)
            return;

        // Calculate direction TO the player
        var direction = (_player.GlobalPosition - GlobalPosition).Normalized();

        // Always move toward the player
        Velocity = direction * Speed;
        MoveAndSlide();
    }


    protected void UpdateSprite(List<string> enemySpritePaths = null)
    {
        //Get random sprite from list
        if (enemySpritePaths != null)
        {
            var randomNumber = GD.RandRange(0, enemySpritePaths.Count - 1);
            var path = enemySpritePaths[randomNumber];
            _sprite.Texture = GD.Load<Texture2D>(path);
        }
    }

    // This method will be called when the enemy takes damage
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        UpdateEnemyHPBar();

        GD.Print($"Kobold took {amount} damage! Health: {CurrentHealth}/{MaxHealth}");

        if (CurrentHealth <= 0) Die();
    }

    private void Die()
    {
        GD.Print("Kobold died!");
        _player?.GainXP(3);
        QueueFree(); // Remove from scene
    }

    protected void UpdateEnemyHPBar()
    {
        if (_healthBar != null)
        {
            _healthBar.MaxValue = MaxHealth;
            _healthBar.Value = CurrentHealth;
        }
    }
}