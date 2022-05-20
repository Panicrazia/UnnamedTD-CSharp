
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class gun : Sprite
{
	 
	public __TYPE turnSpeed = Mathf.Pi;
	
	public void _Process(__TYPE delta)
	{  
		int direction = 0;
		
		if(Input.IsActionPressed("ui_left"))
		{
			direction = -1;
		}
		if(Input.IsActionPressed("ui_right"))
		{
			direction = 1;
			
		}
		rotation += turnSpeed * delta * direction;
		
	
	
	}
	
	
	
}