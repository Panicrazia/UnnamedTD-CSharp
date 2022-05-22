
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class WaveTimer : Timer
{
	public int ticks;
	
	public override void _Ready()
	{  
		Connect("timeout", this, "countdown");
	
	}

	public void Countdown()
	{
		ticks -= 1;
		if (ticks <= 0)
		{
			GD.Print("committing seppuku");
			QueueFree();
		}
	
	}
	
	
	
}