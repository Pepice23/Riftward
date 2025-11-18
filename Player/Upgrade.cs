using Godot;
using System;

public class Upgrade
{
    public string Name;
    public string Description;
    public Action<Player> ApplyEffect;
}