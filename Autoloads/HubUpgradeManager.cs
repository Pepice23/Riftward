using Godot;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class HubUpgradeManager : Node
#pragma warning restore CA1050
{
    public static HubUpgradeManager Instance { get; private set; }
    private readonly List<HubUpgrade> _genericUpgrades = [];
    private readonly List<HubUpgrade> _paladinUpgrades = [];
    private readonly List<HubUpgrade> _mageUpgrades = [];
    private readonly List<HubUpgrade> _hunterUpgrades = [];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        InitializeHubUpgrades();
    }

    private void GenericUpgradesList()
    {
        _genericUpgrades.Add(new HubUpgrade
        {
            Name = "Max Health",
            Description = "Increased Health",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Max Health = {player.MaxHealth}");
                player.MaxHealth = (int)(player.MaxHealth * 1.25f);
                GD.Print($"After upgrade: Max Health = {player.MaxHealth}");
            }
        });

        _genericUpgrades.Add(new HubUpgrade
        {
            Name = "Base Damage",
            Description = "Base Damage",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Base Damage = {player.Damage}");
                player.Damage = (int)(player.Damage * 1.15f);
                GD.Print($"After upgrade: Max Health = {player.Damage}");
            }
        });

        _genericUpgrades.Add(new HubUpgrade
        {
            Name = "Movement Speed",
            Description = "Increased Movement Speed",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Movement Speed = {player.Speed}");
                player.Speed = (int)(player.Speed * 1.10f);
                GD.Print($"After upgrade: Movement Speed = {player.Speed}");
            }
        });

        _genericUpgrades.Add(new HubUpgrade
        {
            Name = "Health Regen",
            Description = "Increased Health Regen",
            MaxRank = 3,
            CostPerRank = [150, 350, 750],
            ApplyEffect = (player) =>
            {
                GD.Print($"Before upgrade: Health Regen = {player.HealthRegen}");
                player.HealthRegen *= 1.5f;
                GD.Print($"After upgrade: Health Regen = {player.HealthRegen}");
            }
        });
    }

    private void MageUpgradesList()
    {
        _mageUpgrades.Add(new HubUpgrade
            {
                Name = "Bolt Speed",
                Description = "Increased Bolt Speed",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Bolt Speed = {player.ProjectileSpeed}");
                    player.ProjectileSpeed = (int)(player.ProjectileSpeed * 1.20f);
                    GD.Print($"After upgrade: Bolt Speed = {player.ProjectileSpeed}");
                }
            }
        );

        _mageUpgrades.Add(new HubUpgrade
            {
                Name = "Starting Bolt Count",
                Description = "+1 Bolt per rank",
                MaxRank = 3,
                CostPerRank = [300, 600, 1200],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Bolt Count = {player.ProjectileCount}");
                    player.ProjectileCount += 1;
                    GD.Print($"After upgrade: Bolt Count = {player.ProjectileCount}");
                }
            }
        );

        _mageUpgrades.Add(new HubUpgrade
            {
                Name = "Attack Cooldown",
                Description = "Decreased Attack Cooldown",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Attack Speed = {player.AttackCooldown}");
                    player.AttackCooldown *= 0.9f;
                    GD.Print($"After upgrade: Attack Speed = {player.AttackCooldown}");
                }
            }
        );
    }

    private void HunterUpgradesList()
    {
        _hunterUpgrades.Add(new HubUpgrade
            {
                Name = "Arrow Speed",
                Description = "Increased Arrow Speed",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Arrow Speed = {player.ProjectileSpeed}");
                    player.ProjectileSpeed = (int)(player.ProjectileSpeed * 1.20f);
                    GD.Print($"After upgrade: Arrow Speed = {player.ProjectileSpeed}");
                }
            }
        );

        _hunterUpgrades.Add(new HubUpgrade
            {
                Name = "Starting Arrow Count",
                Description = "+1 Arrow per rank",
                MaxRank = 3,
                CostPerRank = [300, 600, 1200],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Arrow Count = {player.ProjectileCount}");
                    player.ProjectileCount += 1;
                    GD.Print($"After upgrade: Arrow Count = {player.ProjectileCount}");
                }
            }
        );

        _hunterUpgrades.Add(new HubUpgrade
            {
                Name = "Attack Cooldown",
                Description = "Decreased Attack Cooldown",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) =>
                {
                    GD.Print($"Before upgrade: Attack Speed = {player.AttackCooldown}");
                    player.AttackCooldown *= 0.9f;
                    GD.Print($"After upgrade: Attack Speed = {player.AttackCooldown}");
                }
            }
        );
    }

    private void PaladinUpgradesList()
    {
        _paladinUpgrades.Add(new HubUpgrade
            {
                Name = "Aura Radius",
                Description = "Increased Aura Radius",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = player =>
                {
                    GD.Print($"Before upgrade: Aura radius = {player.AuraRadius}");
                    player.AuraRadius = (int)(player.AuraRadius * 1.15f);
                    player.UpdateHammerPositions(player.AuraRadius);
                    GD.Print($"After upgrade: Aura Radius = {player.AuraRadius}");
                }
            }
        );

        _paladinUpgrades.Add(new HubUpgrade
            {
                Name = "Divine Retribution",
                Description = "Aura heals back for damage dealt",
                MaxRank = 3,
                CostPerRank = [400, 600, 1200],
                ApplyEffect = player =>
                {
                    GD.Print($"Before upgrade: Aura Life Leech = {player.AuraLifeLeech}");
                    player.AuraLifeLeech *= 1.05f;
                    GD.Print($"After upgrade: Aura Life Leech = {player.AuraLifeLeech}");
                }
            }
        );
    }

    private void InitializeHubUpgrades()
    {
        GenericUpgradesList();
        PaladinUpgradesList();
        MageUpgradesList();
        HunterUpgradesList();
    }

    public void ApplyHubUpgrades(Player player)
    {
        foreach (var upgrade in _genericUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
        {
            for (var i = 0; i < upgrade.CurrentRank; i++)
                upgrade.ApplyEffect(player);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Paladin)
        {
            foreach (var upgrade in _paladinUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                    upgrade.ApplyEffect(player);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Mage)
        {
            foreach (var upgrade in _mageUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                {
                    upgrade.ApplyEffect(player);
                }
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Hunter)
            foreach (var upgrade in _hunterUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                {
                    upgrade.ApplyEffect(player);
                }
    }
}