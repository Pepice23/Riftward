using System.Collections.Generic;
using System.Linq;
using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class LevelUpUi : CanvasLayer
#pragma warning restore CA1050
{
    [Export] public Player Player;

    private UpgradeButton _upgradeButton1;
    private UpgradeButton _upgradeButton2;
    private UpgradeButton _upgradeButton3;

    // Store the current 3 upgrades
    private Upgrade _currentUpgrade1;
    private Upgrade _currentUpgrade2;
    private Upgrade _currentUpgrade3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
        if (Player != null) Player.GamePaused += ShowUpgradeUi;

        _upgradeButton1 = GetNode<UpgradeButton>("%UpgradeButton1");
        _upgradeButton2 = GetNode<UpgradeButton>("%UpgradeButton2");
        _upgradeButton3 = GetNode<UpgradeButton>("%UpgradeButton3");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void ShowUpgradeUi()
    {
        var randomUpgrades = GetRandomUpgrades();

        _currentUpgrade1 = randomUpgrades[0];
        _currentUpgrade2 = randomUpgrades[1];
        _currentUpgrade3 = randomUpgrades[2];

        _upgradeButton1.SetUpgradeNameAndDescription(_currentUpgrade1.Name, _currentUpgrade1.Description);
        _upgradeButton2.SetUpgradeNameAndDescription(_currentUpgrade2.Name, _currentUpgrade2.Description);
        _upgradeButton3.SetUpgradeNameAndDescription(_currentUpgrade3.Name, _currentUpgrade3.Description);
        GetTree().Paused = true;
        Show();
    }

    private void HideUpgradeUi()
    {
        Hide();
        GetTree().Paused = false;
    }

    private void OnUpgrade1Selected()
    {
        _currentUpgrade1.ApplyEffect(Player);
        HideUpgradeUi();
    }

    private void OnUpgrade2Selected()
    {
        _currentUpgrade2.ApplyEffect(Player);
        HideUpgradeUi();
    }

    private void OnUpgrade3Selected()
    {
        _currentUpgrade3.ApplyEffect(Player);
        HideUpgradeUi();
    }

    public override void _ExitTree()
    {
        if (Player != null) Player.GamePaused -= ShowUpgradeUi;
    }

    private List<Upgrade> GetRandomUpgrades()
    {
        var randomUpgrades = UpgradeManager.Instance.AllUpgrades
            .OrderBy(_ => GD.Randf()) // Shuffle randomly
            .Take(3) // Take first 3
            .ToList(); // Convert to List

        return randomUpgrades;
    }
}