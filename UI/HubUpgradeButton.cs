using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class HubUpgradeButton : Button
#pragma warning restore CA1050
{
	private Label _upgradeName;
	private Label _upgradeDescription;
	private Label _currentRank;
	private Label _maxRank;
	private Label _costNumber;

	public override void _Ready()
	{
		_upgradeName = GetNode<Label>("%UpgradeName");
		_upgradeDescription = GetNode<Label>("%UpgradeDescription");
		_currentRank = GetNode<Label>("%CurrentRank");
		_maxRank = GetNode<Label>("%MaxRank");
		_costNumber = GetNode<Label>("%CostNumber");
	}

	public void SetUpgradeInfo(string upgradeName, string upgradeDescription, int currentRank, int maxRank, int cost)
	{
		_upgradeName.Text = upgradeName;
		_upgradeDescription.Text = upgradeDescription;
		_currentRank.Text = currentRank.ToString();
		_maxRank.Text = maxRank.ToString();

		if (currentRank == maxRank)
		{
			_costNumber.Text = "MAX RANK";
			Disabled = true;
		}
		else
		{
			_costNumber.Text = cost.ToString();
			Disabled = false;
		}
	}
}
