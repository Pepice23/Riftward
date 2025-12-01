using Godot;
using System;

public partial class MainMenu : VBoxContainer
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
		else{
			GD.Print("Winter mode is disabled");
		}
	}

	private void OnNewGameButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://UI/class_selection.tscn");
	}
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
