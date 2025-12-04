using Godot;
using System;

public partial class LoseScreen : PanelContainer
{
	private void GoBackToMainMenu()
	{
		GetTree().ChangeSceneToFile("res://UI/main_menu.tscn");
	}
}
