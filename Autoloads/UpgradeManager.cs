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
    }

    private void PaladinUpgradeList()
    {
        _paladinUpgrades.Add(new Upgrade
            {
                Name = "Increased Aura Radius",
                Description = "+25 Increased Aura Radius",
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
            Description = "+100 Speed to Bolts",
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
            Description = "-0.05 to Attack Cooldown",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Attack Cooldown = {player.AttackCooldown}");
                player.AttackCooldown -= 0.05f;
                GD.Print($"After upgrade: Attack Cooldown = {player.AttackCooldown}");
            }
        });
    }

    private void HunterUpgradeList()
    {
        _hunterUpgrades.Add(new Upgrade
        {
            Name = "Arrow Speed Boost",
            Description = "+100 Speed to arrow",
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
            Description = "-0.05 to Attack Cooldown",
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Attack Cooldown = {player.AttackCooldown}");
                player.AttackCooldown -= 0.05f;
                GD.Print($"After upgrade: Attack Cooldown = {player.AttackCooldown}");
            }
        });
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