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
    
    private readonly List<string> _enemyWinterSpritePaths =
    [
        "res://Assets/Sprites/enemies/swarm/winter_tree.png", "res://Assets/Sprites/enemies/swarm/winter_ball.png",
        "res://Assets/Sprites/enemies/swarm/winter_imp.png"
    ];

    public override void _Ready()
    {
        base._Ready();
        Speed = 100;
        MaxHealth = 10;
        Damage = 10;

        EnemyName = "Regular Enemy";
        GoldReward = 3;
        
        if (GameManager.Instance.IsWinterModeEnabled)
        {
            UpdateSprite(_enemyWinterSpritePaths); 
        }
        else
        {
            UpdateSprite(_enemySpritePaths);
        }

        CurrentHealth = MaxHealth;
        UpdateEnemyHPBar();
    }
}