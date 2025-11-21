using Godot;


// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class ClassSelection : HBoxContainer
#pragma warning restore CA1050
{
    private void SelectPaladin()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Paladin;
        GetTree().ChangeSceneToFile("res://main.tscn");
        UpgradeManager.Instance.InitializeUpgrades();
    }

    private void SelectMage()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Mage;
        GetTree().ChangeSceneToFile("res://main.tscn");
        UpgradeManager.Instance.InitializeUpgrades();
    }

    private void SelectHunter()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Hunter;
        GetTree().ChangeSceneToFile("res://main.tscn");
        UpgradeManager.Instance.InitializeUpgrades();
    }
}