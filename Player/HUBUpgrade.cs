using System;

// ReSharper disable once CheckNamespace

#pragma warning disable CA1050
public class HubUpgrade
#pragma warning restore CA1050
{
    public string Name;
    public string Description;
    public int CurrentRank = 0;
    public int MaxRank;
    public int[] CostPerRank;
    public Action<Player> ApplyEffect;
}