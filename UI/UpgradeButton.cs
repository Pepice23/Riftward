using Godot;
using System;

#pragma warning disable CA1050
public partial class UpgradeButton : Button
#pragma warning restore CA1050
{
    private Label _upgradeName;
    private Label _upgradeDescription;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _upgradeName = GetNode<Label>("%UpgradeName");
        _upgradeDescription = GetNode<Label>("%UpgradeDescription");
    }

    public void SetUpgradeNameAndDescription(string upgradeName, string upgradeDescription)
    {
        _upgradeName.Text = upgradeName;
        _upgradeDescription.Text = upgradeDescription;
    }
}