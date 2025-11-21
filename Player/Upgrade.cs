using System;

// ReSharper disable once CheckNamespace

#pragma warning disable CA1050
public class Upgrade
#pragma warning restore CA1050
{
    public string Name;
    public string Description;
    public Action<Player> ApplyEffect;
}