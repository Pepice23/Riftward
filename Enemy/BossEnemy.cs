using System.Collections.Generic;


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

    public override void _Ready()
    {
        base._Ready();
        Speed = 50;
        MaxHealth = 200;
        Damage = 30;
        UpdateSprite(_enemySpritePaths);
        CurrentHealth = MaxHealth;
        UpdateEnemyHPBar();
    }
}