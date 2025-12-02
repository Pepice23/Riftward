using Godot;

#pragma warning disable CA1050
// ReSharper disable once CheckNamespace
public partial class GameManager : Node
#pragma warning restore CA1050
{
    [Signal]
    public delegate void TimeUpdatedEventHandler(float time);
    
    [Signal]
    public delegate void BossTimeEventHandler();

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
    public int MaxRunTime = 300;
    public PlayerClass SelectedClass = PlayerClass.Paladin;
    public bool IsWinterModeEnabled = false;
    public bool Victory = false;


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
            if (RunTime >= MaxRunTime)
            {
                EmitSignal(SignalName.BossTime);
                EndRun();
                
            }

            // Check if 10 seconds have passed since last update
            if (RunTime - _lastUpdateTime >= 10f)
            {
                CurrentEnemyMaxHealth += 3;
                CurrentEliteEnemyMaxHealth += 3;
                _lastUpdateTime = RunTime; // Remember this time for next check
            }
        }
    }

    public void EndRun()
    {
        IsRunActive = false; // Stop counting
        // Stop normal and elite spawning
        var enemies = GetTree().GetNodesInGroup("normal_enemies");
        foreach (var enemy in enemies)
        {
            enemy.QueueFree();
        }

        if (Victory)
        {
            GD.Print("Victory! You survived 5 minutes! and Defeated the Boss!");
            
            // Later: Show victory screen
        }
    }

    public void StartRun()
    {
        IsRunActive = true;
        RunTime = 0f;
    }
}