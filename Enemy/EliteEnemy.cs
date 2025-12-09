using System.Collections.Generic;


#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class EliteEnemy : BaseEnemy
#pragma warning restore CA1050
{
    private readonly List<string> _enemySpritePaths =
    [
        "res://Assets/Sprites/enemies/elite/death_knight.png",
        "res://Assets/Sprites/enemies/elite/corrupted_paladin.png",
        "res://Assets/Sprites/enemies/elite/fel_warlock.png", "res://Assets/Sprites/enemies/elite/beastmaster.png"
    ];
    private readonly List<string> _enemyWinterSpritePaths =
    [
        "res://Assets/Sprites/enemies/elite/winter_gingerbread.png",
        "res://Assets/Sprites/enemies/elite/winter_warlock.png",
        "res://Assets/Sprites/enemies/elite/winter_reindeer.png", 
    ];

    public override void _Ready()
    {
        base._Ready();
        Speed = 70;
        MaxHealth = 40;
        Damage = 20;

        EnemyName = "Elite Enemy";
        GoldReward = 20;
        
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