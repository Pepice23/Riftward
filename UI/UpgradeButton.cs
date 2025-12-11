using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class UpgradeButton : Button
#pragma warning restore CA1050
{
    private Label _upgradeName;
    private Label _upgradeDescription;
    private Label _currentRank;
    private Label _maxRank;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _upgradeName = GetNode<Label>("%UpgradeName");
        _upgradeDescription = GetNode<Label>("%UpgradeDescription");
        _currentRank = GetNode<Label>("%CurrentRank");
        _maxRank = GetNode<Label>("%MaxRank");
    }

    public void SetUpgradeNameAndDescription(string upgradeName, string upgradeDescription, int currentRank,
        int maxRank)
    {
        _upgradeName.Text = upgradeName;
        _upgradeDescription.Text = upgradeDescription;
        _currentRank.Text = currentRank.ToString();
        _maxRank.Text = maxRank.ToString();
    }
}