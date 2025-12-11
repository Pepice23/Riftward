using Godot;
using System;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class WinScreen : PanelContainer
#pragma warning restore CA1050
{
    private Label _currentRunGold;

    public override void _Ready()
    {
        _currentRunGold = GetNode<Label>("%RunGoldLabel");
        UpdateRunGold(GameManager.Instance.RunGold);
    }

    private void GoBackToMainMenu()
    {
        GetTree().ChangeSceneToFile("res://UI/hub.tscn");
        GameManager.Instance.ResetGame();
    }

    private void UpdateRunGold(int runGold)
    {
        _currentRunGold.Text = runGold.ToString();
    }
}