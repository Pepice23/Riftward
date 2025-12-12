using Godot;

// ReSharper disable once CheckNamespace
public partial class Player
{
    // Calculate how much XP is needed for the next level
    private void CalculateXPNeeded()
    {
        // Formula: Base * (1.15 ^ (Level - 1))
        // Level 2: 10 * 1.15^0 = 10
        // Level 3: 10 * 1.15^1 = 11.5 → 12
        // Level 4: 10 * 1.15^2 = 13.2 → 14
        _xpNeededForNextLevel = Mathf.CeilToInt(BaseXPNeeded * Mathf.Pow(1.43f, CurrentLevel - 1));
        EmitSignal(SignalName.XPUpdated, _xpNeededForNextLevel, CurrentXP);
    }

    // Call this when player should gain XP
    public void GainXP(int amount)
    {
        CurrentXP += amount;
        GD.Print($"Gained {amount} XP! Total: {CurrentXP}/{_xpNeededForNextLevel}");

        // Did we level up?
        if (CurrentXP >= _xpNeededForNextLevel) LevelUp();

        EmitSignal(SignalName.XPUpdated, _xpNeededForNextLevel, CurrentXP);
    }

    private void LevelUp()
    {
        // Subtract the XP cost (carry over extra XP)
        CurrentXP -= _xpNeededForNextLevel;

        // Increase level
        CurrentLevel++;

        // Recalculate XP needed for next level
        CalculateXPNeeded();
        EmitSignal(SignalName.LevelUpdated, CurrentLevel);


        PauseGame();
    }

    private void PauseGame()
    {
        EmitSignal(SignalName.GamePaused);
    }
}