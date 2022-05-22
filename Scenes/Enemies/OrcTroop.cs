
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class OrcTroop : Baddie
{
	public static readonly Vector2 BASE_VELOCITY = new Vector2(25f, 0.0f);
	
	public override void _Ready()
	{
		velocity = BASE_VELOCITY;
	}
}