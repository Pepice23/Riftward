using Godot;

public partial class Hud : CanvasLayer
{
    [Export] public Player Player;

    private TextureButton _changeToPaladin;
    private TextureButton _changeToMage;
    private TextureButton _changeToHunter;
    private ProgressBar _xpBar;
    private Label _currentXpLabel;
    private Label _maxXpLabel;
    private Label _levelNumber;

    // Define the signal at class level
    [Signal]
    public delegate void PaladinSelectedEventHandler();

    [Signal]
    public delegate void MageSelectedEventHandler();

    [Signal]
    public delegate void HunterSelectedEventHandler();

    public override void _Ready()
    {
        _changeToPaladin = GetNode<TextureButton>("%PaladinButton");
        _changeToMage = GetNode<TextureButton>("%MageButton");
        _changeToHunter = GetNode<TextureButton>("%HunterButton");
        _xpBar = GetNode<ProgressBar>("%XPProgressBar");
        _currentXpLabel = GetNode<Label>("%CurrentXP");
        _maxXpLabel = GetNode<Label>("%MaxXP");
        _levelNumber = GetNode<Label>("%LevelNumber");
        if (Player != null)
        {
            Player.XPUpdated += UpdateXP;
            Player.LevelUpdated += UpdateLevelNumber;
        }
    }

    private void OnPaladinButtonPressed()
    {
        // Emit the signal with the character name
        EmitSignal(SignalName.PaladinSelected);
    }

    private void OnMageButtonPressed()
    {
        // Emit the signal with the character name
        EmitSignal(SignalName.MageSelected);
    }

    private void OnHunterButtonPressed()
    {
        // Emit the signal with the character name
        EmitSignal(SignalName.HunterSelected);
    }

    private void UpdateXP(int xpNeededForNextLevel, int currentXP)
    {
        _xpBar.MaxValue = xpNeededForNextLevel;
        _xpBar.Value = currentXP;
        _currentXpLabel.Text = currentXP.ToString();
        _maxXpLabel.Text = xpNeededForNextLevel.ToString();
    }

    private void UpdateLevelNumber(int levelNumber)
    {
        _levelNumber.Text = levelNumber.ToString();
    }

    public override void _ExitTree()
    {
        if (Player != null)
        {
            Player.XPUpdated -= UpdateXP;
            Player.LevelUpdated -= UpdateLevelNumber;
        }
    }
}