using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class Hud : CanvasLayer
#pragma warning restore CA1050
{
    [Export] public Player Player;


    private ProgressBar _xpBar;
    private Label _currentXpLabel;
    private Label _maxXpLabel;
    private Label _levelNumber;
    private ProgressBar _timerBar;
    private Label _currentTime;


    public override void _Ready()
    {
        _xpBar = GetNode<ProgressBar>("%XPProgressBar");
        _currentXpLabel = GetNode<Label>("%CurrentXP");
        _maxXpLabel = GetNode<Label>("%MaxXP");
        _levelNumber = GetNode<Label>("%LevelNumber");
        _timerBar = GetNode<ProgressBar>("%TimerProgressBar");
        _currentTime = GetNode<Label>("%SecondsNumber");
        if (Player != null)
        {
            Player.XPUpdated += UpdateXP;
            Player.LevelUpdated += UpdateLevelNumber;
        }

        if (GameManager.Instance != null) GameManager.Instance.TimeUpdated += UpdateTimerBar;
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

    private void UpdateTimerBar(float seconds)
    {
        _timerBar.MaxValue = 600;
        _timerBar.Value = seconds;
        var mins = (int)(seconds / 60);
        var secs = (int)(seconds % 60);
        _currentTime.Text = $"{mins}:{secs:D2}";
    }

    public override void _ExitTree()
    {
        if (Player != null)
        {
            Player.XPUpdated -= UpdateXP;
            Player.LevelUpdated -= UpdateLevelNumber;
        }

        if (GameManager.Instance != null) GameManager.Instance.TimeUpdated -= UpdateTimerBar;
    }
}