using System;

// ReSharper disable once CheckNamespace

public class Upgrade
{
    public string Name;
    public string Description;
    public Action<Player> ApplyEffect;
}