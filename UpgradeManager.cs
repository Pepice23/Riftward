using Godot;
using System.Collections.Generic;

public partial class UpgradeManager : Node
{
    public static UpgradeManager Instance;
    public List<Upgrade> AllUpgrades = new();

    public override void _Ready()
    {
        Instance = this;
    }
}