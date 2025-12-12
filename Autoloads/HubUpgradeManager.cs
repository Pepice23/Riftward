using Godot;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class HubUpgradeManager : Node
#pragma warning restore CA1050
{
    public static HubUpgradeManager Instance { get; private set; }
    public List<HubUpgrade> GenericUpgrades { get; } = [];
    public List<HubUpgrade> PaladinUpgrades { get; } = [];
    public List<HubUpgrade> MageUpgrades { get; } = [];
    public List<HubUpgrade> HunterUpgrades { get; } = [];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        InitializeHubUpgrades();
    }

    private void GenericUpgradesList()
    {
        GenericUpgrades.Add(new HubUpgrade
        {
            Name = "Max Health",
            Description = "Increased Health",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) => { player.MaxHealth = (int)(player.MaxHealth * 1.25f); }
        });

        GenericUpgrades.Add(new HubUpgrade
        {
            Name = "Base Damage",
            Description = "Base Damage",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) => { player.Damage = (int)(player.Damage * 1.15f); }
        });

        GenericUpgrades.Add(new HubUpgrade
        {
            Name = "Movement Speed",
            Description = "Increased Movement Speed",
            MaxRank = 4,
            CostPerRank = [100, 250, 500, 1000],
            ApplyEffect = (player) => { player.Speed = (int)(player.Speed * 1.10f); }
        });

        GenericUpgrades.Add(new HubUpgrade
        {
            Name = "Health Regen",
            Description = "Increased Health Regen",
            MaxRank = 3,
            CostPerRank = [150, 350, 750],
            ApplyEffect = (player) => { player.HealthRegen *= 1.5f; }
        });
    }

    private void MageUpgradesList()
    {
        MageUpgrades.Add(new HubUpgrade
            {
                Name = "Bolt Speed",
                Description = "Increased Bolt Speed",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) => { player.ProjectileSpeed = (int)(player.ProjectileSpeed * 1.20f); }
            }
        );

        MageUpgrades.Add(new HubUpgrade
            {
                Name = "Starting Bolt Count",
                Description = "+1 Bolt per rank",
                MaxRank = 3,
                CostPerRank = [300, 600, 1200],
                ApplyEffect = (player) => { player.ProjectileCount += 1; }
            }
        );

        MageUpgrades.Add(new HubUpgrade
            {
                Name = "Attack Cooldown",
                Description = "Decreased Attack Cooldown",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) => { player.AttackCooldown *= 0.9f; }
            }
        );
    }

    private void HunterUpgradesList()
    {
        HunterUpgrades.Add(new HubUpgrade
            {
                Name = "Arrow Speed",
                Description = "Increased Arrow Speed",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) => { player.ProjectileSpeed = (int)(player.ProjectileSpeed * 1.20f); }
            }
        );

        HunterUpgrades.Add(new HubUpgrade
            {
                Name = "Starting Arrow Count",
                Description = "+1 Arrow per rank",
                MaxRank = 3,
                CostPerRank = [300, 600, 1200],
                ApplyEffect = (player) => { player.ProjectileCount += 1; }
            }
        );

        HunterUpgrades.Add(new HubUpgrade
            {
                Name = "Attack Cooldown",
                Description = "Decreased Attack Cooldown",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = (player) => { player.AttackCooldown *= 0.9f; }
            }
        );
    }

    private void PaladinUpgradesList()
    {
        PaladinUpgrades.Add(new HubUpgrade
            {
                Name = "Aura Radius",
                Description = "Increased Aura Radius",
                MaxRank = 3,
                CostPerRank = [200, 400, 800],
                ApplyEffect = player =>
                {
                    player.AuraRadius = (int)(player.AuraRadius * 1.15f);
                    player.UpdateHammerPositions(player.AuraRadius);
                }
            }
        );

        PaladinUpgrades.Add(new HubUpgrade
            {
                Name = "Divine Retribution",
                Description = "Aura heals back for damage dealt",
                MaxRank = 3,
                CostPerRank = [400, 600, 1200],
                ApplyEffect = player => { player.AuraLifeLeech += 0.05f; }
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
        foreach (var upgrade in GenericUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
        {
            for (var i = 0; i < upgrade.CurrentRank; i++)
                upgrade.ApplyEffect(player);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Paladin)
        {
            foreach (var upgrade in PaladinUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                    upgrade.ApplyEffect(player);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Mage)
        {
            foreach (var upgrade in MageUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                {
                    upgrade.ApplyEffect(player);
                }
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Hunter)
            foreach (var upgrade in HunterUpgrades.Where(upgrade => upgrade.CurrentRank > 0))
                for (var i = 0; i < upgrade.CurrentRank; i++)
                {
                    upgrade.ApplyEffect(player);
                }
    }
}