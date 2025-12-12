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
            MaxRank = 5,
            ApplyEffect = (player) => { player.Damage += 5; }
        });

        AllUpgrades.Add(new Upgrade
        {
            Name = "Speed Boost",
            Description = "Increased Movement Speed",
            MaxRank = 4,
            ApplyEffect = (player) => { player.Speed += 25; }
        });
        AllUpgrades.Add(new Upgrade
        {
            Name = "Health Regeneration",
            Description = "Increased Health Regeneration",
            MaxRank = 5,
            ApplyEffect = (player) => { player.HealthRegen += 0.01f; }
        });
    }

    private void PaladinUpgradeList()
    {
        _paladinUpgrades.Add(new Upgrade
            {
                Name = "Increased Aura Radius",
                Description = "Increased Aura Radius",
                MaxRank = 4,
                ApplyEffect = player =>
                {
                    player.AuraRadius += 25;
                    player.UpdateHammerPositions(player.AuraRadius);
                }
            }
        );

        _paladinUpgrades.Add(new Upgrade
        {
            Name = "Blazing Aura",
            Description = "Aura damages enemies more frequently",
            MaxRank = 3,
            ApplyEffect = player => { player.AuraDamageCooldown -= 0.05f; }
        });

        _paladinUpgrades.Add(new Upgrade
        {
            Name = "Divine Retribution",
            Description = "Aura heals back for damage dealt",
            MaxRank = 3,
            ApplyEffect = player => { player.AuraLifeLeech += 0.1f; }
        });
    }

    private void MageUpgradeList()
    {
        _mageUpgrades.Add(new Upgrade
        {
            Name = "Bolt Speed Boost",
            Description = "Increased Bolt Speed",
            MaxRank = 3,
            ApplyEffect = (player) => { player.ProjectileSpeed += 100; }
        });

        _mageUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "Decreased Attack Cooldown",
            MaxRank = 3,
            ApplyEffect = (player) => { player.AttackCooldown -= 0.05f; }
        });
        _mageUpgrades.Add(new Upgrade
            {
                Name = "+1 Bolt",
                Description = "Shoot 1 more bolt at the same time",
                MaxRank = 3,
                ApplyEffect = player => { player.ProjectileCount += 1; }
            }
        );
    }

    private void HunterUpgradeList()
    {
        _hunterUpgrades.Add(new Upgrade
        {
            Name = "Arrow Speed Boost",
            Description = "Arrow Speed Increased",
            MaxRank = 3,
            ApplyEffect = (player) => { player.ProjectileSpeed += 100; }
        });

        _hunterUpgrades.Add(new Upgrade
        {
            Name = "Faster Shooting",
            Description = "Decreased Attack Cooldown",
            MaxRank = 3,
            ApplyEffect = (player) => { player.AttackCooldown -= 0.05f; }
        });
        _hunterUpgrades.Add(new Upgrade
            {
                Name = "+1 Arrow",
                Description = "Shoot 1 more arrow at the same time",
                MaxRank = 3,
                ApplyEffect = player => { player.ProjectileCount += 1; }
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
            foreach (var upgrade in _paladinUpgrades) AllUpgrades.Add(upgrade);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Mage)
        {
            MageUpgradeList();
            foreach (var upgrade in _mageUpgrades) AllUpgrades.Add(upgrade);
        }

        if (GameManager.Instance.SelectedClass == GameManager.PlayerClass.Hunter)
        {
            HunterUpgradeList();
            foreach (var upgrade in _hunterUpgrades) AllUpgrades.Add(upgrade);
        }
    }

    public void ResetUpgradeLists()
    {
        AllUpgrades.Clear();
        _paladinUpgrades.Clear();
        _mageUpgrades.Clear();
        _hunterUpgrades.Clear();
        GD.Print("All Upgrades Reset!");
    }
}