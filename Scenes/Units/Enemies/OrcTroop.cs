
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class OrcTroop : Baddie
{
	public static readonly Vector2 BASE_VELOCITY = new Vector2(25f, 0.0f);
	//probs have a master static thing with the base values for all creatures so I dont have to do this manually every time I make a new enemy
	public float baseHealth = 10;
	public float baseArmor = 0;
	public float baseSpeed = 25;

	public override void _Ready()
	{
		health = baseHealth;
		armor = baseArmor;
		speed = baseSpeed;
		velocity = BASE_VELOCITY;
	}
}