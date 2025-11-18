using Godot;
using System.Collections.Generic;

public partial class UpgradeManager : Node
{
    public static UpgradeManager Instance;
    public List<Upgrade> AllUpgrades = [];

    public override void _Ready()
    {
        Instance = this;
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        // Create upgrade instances and add them to the list

        AllUpgrades.Add(new Upgrade
        {
            Name = "Damage Boost",
            Description = "+5 Damage",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Damage = {player.Damage}");
                player.Damage += 5;
                GD.Print($"After upgrade: Damage = {player.Damage}");
            }
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Speed Boost",
            Description = "+50 Movement Speed",
            ApplyEffect = (player) => player.Speed += 50
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Projectile Speed Boost",
            Description = "+100 Speed to projectiles",
            ApplyEffect = (player) => player.ProjectileSpeed += 100
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "-0.05 to Attack Cooldown",
            ApplyEffect = (player) => player.AttackCooldown -= 0.05f
        });
    }
}