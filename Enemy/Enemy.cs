using System.Collections.Generic;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class Enemy : BaseEnemy
#pragma warning restore CA1050
{
    private readonly List<string> _enemySpritePaths =
    [
        "res://Assets/Sprites/enemies/swarm/bandit.png", "res://Assets/Sprites/enemies/swarm/gnoll.png",
        "res://Assets/Sprites/enemies/swarm/kobold.png"
    ];

    public override void _Ready()
    {
        base._Ready();
        Speed = 100;
        MaxHealth = 10;
        Damage = 10;
        UpdateSprite(_enemySpritePaths);
        CurrentHealth = MaxHealth;
        UpdateEnemyHPBar();
    }
}