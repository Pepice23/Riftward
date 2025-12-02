using System.Collections.Generic;
using Godot;


#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class BossEnemy : BaseEnemy
#pragma warning restore CA1050
{
    private readonly List<string> _enemySpritePaths =
    [
        "res://Assets/Sprites/enemies/boss/abyssal_tyrant.png",
        "res://Assets/Sprites/enemies/boss/void_star.png",
        "res://Assets/Sprites/enemies/boss/lich_king.png", "res://Assets/Sprites/enemies/boss/dragon_lord.png"
    ];
    private readonly List<string> _enemyWinterSpritePaths =
    [
        "res://Assets/Sprites/enemies/boss/winter_xmas_tree.png",
        "res://Assets/Sprites/enemies/boss/winter_yule_lord.png",
        "res://Assets/Sprites/enemies/boss/winter_lich.png", 
    ];

    public override void _Ready()
    {
        base._Ready();
        Speed = 50;
        MaxHealth = 200;
        Damage = 30;
        
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
    
    protected override void Die()
    {
        GD.Print("Boss died!");
        GameManager.Instance.Victory =  true;
        QueueFree(); // Remove from scene
    }
}