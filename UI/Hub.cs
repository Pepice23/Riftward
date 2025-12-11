using Godot;
using System;

// ReSharper disable once CheckNamespace
#pragma warning disable CA1050
public partial class Hub : Node2D
#pragma warning restore CA1050
{
    private TextureButton _paladinButton;

    private TextureButton _mageButton;

    private TextureButton _hunterButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _paladinButton = GetNode<TextureButton>("PaladinTrainer");
        _mageButton = GetNode<TextureButton>("MageTrainer");
        _hunterButton = GetNode<TextureButton>("HunterTrainer");

        _paladinButton.MouseEntered += OnPaladinHoverEnter;
        _mageButton.MouseEntered += OnMageHoverEnter;
        _hunterButton.MouseEntered += OnHunterHoverEnter;

        _paladinButton.MouseExited += OnPaladinHoverExit;
        _mageButton.MouseExited += OnMageHoverExit;
        _hunterButton.MouseExited += OnHunterHoverExit;
    }

    private void OnPaladinHoverEnter()
    {
        _paladinButton.Modulate = new Color(1.2f, 1.2f, 1.2f);
    }

    private void OnMageHoverEnter()
    {
        _mageButton.Modulate = new Color(1.2f, 1.2f, 1.2f);
    }

    private void OnHunterHoverEnter()
    {
        _hunterButton.Modulate = new Color(1.2f, 1.2f, 1.2f);
    }

    private void OnPaladinHoverExit()
    {
        _paladinButton.Modulate = Colors.White;
    }

    private void OnMageHoverExit()
    {
        _mageButton.Modulate = Colors.White;
    }

    private void OnHunterHoverExit()
    {
        _hunterButton.Modulate = Colors.White;
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
        GetTree().ChangeSceneToFile("res://UI/class_selection.tscn");
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }

    public override void _ExitTree()
    {
        _paladinButton.MouseEntered -= OnPaladinHoverEnter;
        _paladinButton.MouseExited -= OnPaladinHoverExit;
        _mageButton.MouseEntered -= OnMageHoverEnter;
        _mageButton.MouseExited -= OnMageHoverExit;
        _hunterButton.MouseEntered -= OnHunterHoverEnter;
        _hunterButton.MouseExited -= OnHunterHoverExit;
    }
}