using Godot;


// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class MainMenu : VBoxContainer
#pragma warning restore CA1050
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private void OnWinterModeButtonPressed(bool isChecked)
    {
        GameManager.Instance.IsWinterModeEnabled = isChecked;
        if (GameManager.Instance.IsWinterModeEnabled)
        {
            GD.Print("Winter mode is enabled");
        }
        else
        {
            GD.Print("Winter mode is disabled");
        }
    }

    private void OnNewGameButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://UI/hub.tscn");
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}