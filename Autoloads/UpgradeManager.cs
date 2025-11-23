using Godot;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class UpgradeManager : Node
#pragma warning restore CA1050
{
    public static UpgradeManager Instance { get; private set; }
    public readonly List<Upgrade> AllUpgrades = [];
    private readonly List<Upgrade> _paladinUpgrades = [];
    private readonly List<Upgrade> _mageUpgrades = [];
    private readonly List<Upgrade> _hunterUpgrades = [];


    public override void _Ready()
    {
        Instance = this;
    }

    private void AllUpgradeList()
    {
        AllUpgrades.Add(new Upgrade
        {
            Name = "Damage Boost",
            Description = "Increased Damage",
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
            Description = "Increased Movement Speed",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Movement Speed = {player.Speed}");
                player.Speed += 50;
                GD.Print($"After upgrade: Movement Speed = {player.Speed}");
            }
        });
        AllUpgrades.Add(new Upgrade
        {
            Name = "Health Regeneration",
            Description = "Increased Health Regeneration",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Movement Speed = {player.HealthRegen}");
                player.HealthRegen += 0.01f;
                GD.Print($"After upgrade: Movement Speed = {player.HealthRegen}");
            }
        });
    }

    private void PaladinUpgradeList()
    {
        _paladinUpgrades.Add(new Upgrade
            {
                Name = "Increased Aura Radius",
                Description = "Increased Aura Radius",
                ApplyEffect = player =>
                {
                    GD.Print($"Before upgrade: Aura radius = {player.AuraRadius}");
                    player.AuraRadius += 25;
                    player.UpdateHammerPositions(player.AuraRadius);
                    GD.Print($"After upgrade: Aura Radius = {player.AuraRadius}");
                }
            }
        );
    }

    private void MageUpgradeList()
    {
        _mageUpgrades.Add(new Upgrade
        {
            Name = "Bolt Speed Boost",
            Description = "Increased Bolt Speed",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Bolt Speed = {player.ProjectileSpeed}");
                player.ProjectileSpeed += 100;
                GD.Print($"After upgrade: Bolt Speed = {player.ProjectileSpeed}");
            }
        });

        _mageUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "Decreased Attack Cooldown",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Attack Cooldown = {player.AttackCooldown}");
                player.AttackCooldown -= 0.05f;
                GD.Print($"After upgrade: Attack Cooldown = {player.AttackCooldown}");
            }
        });
        _mageUpgrades.Add(new Upgrade
            {
                Name = "+1 Bolt",
                Description = "Shoot 1 more bolt at the same time",
                ApplyEffect = player =>
                {
                    GD.Print($"Before upgrade: Projectile Count = {player.ProjectileCount}");
                    player.ProjectileCount += 1;
                    GD.Print($"After upgrade: Projectile Count = {player.ProjectileCount}");
                }
            }
        );
    }

    private void HunterUpgradeList()
    {
        _hunterUpgrades.Add(new Upgrade
        {
            Name = "Arrow Speed Boost",
            Description = "Arrow Speed Increased",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Arrow Speed = {player.ProjectileSpeed}");
                player.ProjectileSpeed += 100;
                GD.Print($"After upgrade: Arrow Speed = {player.ProjectileSpeed}");
            }
        });

        _hunterUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "Decreased Attack Cooldown",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Attack Cooldown = {player.AttackCooldown}");
                player.AttackCooldown -= 0.05f;
                GD.Print($"After upgrade: Attack Cooldown = {player.AttackCooldown}");
            }
        });
        _hunterUpgrades.Add(new Upgrade
            {
                Name = "+1 Arrow",
                Description = "Shoot 1 more arrow at the same time",
                ApplyEffect = player =>
                {
                    GD.Print($"Before upgrade: Projectile Count = {player.ProjectileCount}");
                    player.ProjectileCount += 1;
                    GD.Print($"After upgrade: Projectile Count = {player.ProjectileCount}");
                }
            }
        );
    }

    public void InitializeUpgrades()
    {
        GD.Print($"Selected class: {GameManager.Instance.SelectedClass}");
        AllUpgradeList();
        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Paladin)
        {
            PaladinUpgradeList();
            foreach (var upgrade in _paladinUpgrades)
            {
                AllUpgrades.Add(upgrade);
            }
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Mage)
        {
            MageUpgradeList();
            foreach (var upgrade in _mageUpgrades)
            {
                AllUpgrades.Add(upgrade);
            }
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Hunter)
        {
            HunterUpgradeList();
            foreach (var upgrade in _hunterUpgrades)
            {
                AllUpgrades.Add(upgrade);
            }
        }
    }
}