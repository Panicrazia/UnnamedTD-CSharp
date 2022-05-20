
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class OrcTroop : "res://Scenes/Enemies/baddie.gd"
{
	 
	public static readonly Vector2 BASEVelocity = new Vector2(25, 0.0);
	//public void _PhysicsProcess(__TYPE delta)
	{  
	//	position += velocity * delta * speed;
	}
	
	public void _Ready()
	{  
		velocity = BASEVelocity;
	
	
	}
	
	
	
}