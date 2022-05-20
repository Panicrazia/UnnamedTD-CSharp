
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class WaveTimer : Timer
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	var countdownVar
	
	public void _Ready()
	{  
		Connect("timeout", this, "countdown");
	
	}
	
	public void Countdown()
	{  
		countdownVar-=1;
		if countdownVar <= 0:
			GD.Print("committing seppuku");
			QueueFree();
	
	
	}
	
	
	
}