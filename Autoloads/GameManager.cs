using Godot;

#pragma warning disable CA1050
public partial class GameManager : Node
#pragma warning restore CA1050
{
    [Signal]
    public delegate void TimeUpdatedEventHandler(float time);

    public enum PlayerClass
    {
        Paladin,
        Mage,
        Hunter
    }

    public static GameManager Instance { get; private set; }
    public bool IsRunActive;
    public float RunTime;
    public int CurrentEnemyMaxHealth = 10;
    public int CurrentEliteEnemyMaxHealth = 40;
    public PlayerClass SelectedClass = PlayerClass.Paladin;


    private float _lastUpdateTime; // Track when we last updated


    public override void _Ready()
    {
        Instance = this;
    }

    public override void _Process(double delta)
    {
        if (IsRunActive)
        {
            RunTime += (float)delta;
            EmitSignal(SignalName.TimeUpdated, RunTime);
            if (RunTime >= 600f) EndRun(true);

            // Check if 10 seconds have passed since last update
            if (RunTime - _lastUpdateTime >= 10f)
            {
                CurrentEnemyMaxHealth += 3;
                CurrentEliteEnemyMaxHealth += 3;
                _lastUpdateTime = RunTime; // Remember this time for next check
            }
        }
    }

    public void EndRun(bool victory)
    {
        IsRunActive = false; // Stop counting

        if (victory)
            GD.Print("Victory! You survived 10 minutes!");
        // Later: Show victory screen
        else
            GD.Print("Game Over!");
        // Later: Show game over screen
    }

    public void StartRun()
    {
        IsRunActive = true;
        RunTime = 0f;
    }
}