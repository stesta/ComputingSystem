using Godot;
using System;
using System.Diagnostics;

using ComputingSystem.Components;

public partial class Computer : Node2D
{
    private Memory RAM = new();
    private Memory ROM = new();
    private CPU CPU = new();

    public void Tick()
    {
        Debug.WriteLine("tick");
    }
}
