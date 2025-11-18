using Godot;

public partial class GameManager : Node
{
    [Signal]
    public delegate void TimeUpdatedEventHandler(float time);

    public static GameManager Instance { get; private set; }
    public bool IsRunActive;
    public float RunTime;

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
            if (RunTime >= 600f)
            {
                EndRun(true);
            }
        }
    }

    public void EndRun(bool victory)
    {
        IsRunActive = false; // Stop counting

        if (victory)
        {
            GD.Print("Victory! You survived 10 minutes!");
            // Later: Show victory screen
        }
        else
        {
            GD.Print("Game Over!");
            // Later: Show game over screen
        }
    }

    public void StartRun()
    {
        IsRunActive = true;
        RunTime = 0f;
    }
}