using Godot;


public partial class ClassSelection : HBoxContainer
{
    private void SelectPaladin()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Paladin;
        GetTree().ChangeSceneToFile("res://main.tscn");
    }

    private void SelectMage()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Mage;
        GetTree().ChangeSceneToFile("res://main.tscn");
    }

    private void SelectHunter()
    {
        GameManager.Instance.SelectedClass = GameManager.PlayerClass.Hunter;
        GetTree().ChangeSceneToFile("res://main.tscn");
    }
}