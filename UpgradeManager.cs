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
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Movement Speed = {player.Speed}");
                player.Speed += 50;
                GD.Print($"After upgrade: Movement Speed = {player.Speed}");
            }
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Projectile Speed Boost",
            Description = "+100 Speed to projectiles",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Projectile Speed = {player.ProjectileSpeed}");
                player.ProjectileSpeed += 100;
                GD.Print($"After upgrade: Projectile Speed = {player.ProjectileSpeed}");
            }
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "-0.05 to Attack Cooldown",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Attack Cooldown = {player.AttackCooldown}");
                player.AttackCooldown -= 0.05f;
                GD.Print($"After upgrade: Attack Cooldown = {player.AttackCooldown}");
            }
        });
    }
}