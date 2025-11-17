using Godot;

public partial class LevelUpUi : CanvasLayer
{
    [Export] public Player Player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
        if (Player != null)
        {
            Player.GamePaused += ShowUpgradeUi;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void ShowUpgradeUi()
    {
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
        GD.Print("Player chose Upgrade 1");
        HideUpgradeUi();
    }

    private void OnUpgrade2Selected()
    {
        GD.Print("Player chose Upgrade 2");
        HideUpgradeUi();
    }

    private void OnUpgrade3Selected()
    {
        GD.Print("Player chose Upgrade 3");
        HideUpgradeUi();
    }

    public override void _ExitTree()
    {
        if (Player != null)
        {
            Player.GamePaused -= ShowUpgradeUi;
        }
    }
}