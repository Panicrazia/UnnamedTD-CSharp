
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using System.Collections.Generic;

public class Baddie : Node2D
{
	//TODO: make an enemy handler in c++ so the game will slow to a pathetic crawl at the slowest rate possible
	public const int CELLSize = 16;
	//health && whatnot should be changed to consts to have the base health/armor/whatnot
	public float health = 10;
	public float armor = 0;
	public float speed = 25;
	public object[] effects  = new object[0];
	public Vector2 velocity = Vector2.Zero;
	public Dictionary<Vector2, object[]> path;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{  
		
	}
	
	public void Damage(float damageDone)
	{  
		damageDone -= armor;
		if(damageDone > 0)
		{
			health -= damageDone;
			if(health < 0)
			{
				QueueFree();
	
			}
		}
	}
	
	public void DoEffects(object[] effects)
	{  
		//print("Baddie got hit by a projectle")
	}
	
	public override void _PhysicsProcess(float delta)
	{  
//		var adjustedPosition = position + GetParent().GetParent().position;
//		var goal = path[new Vector2(Mathf.Floor(adjustedPosition.x/16 as int), Mathf.Floor(adjustedPosition.y/16 as int))];
//		LookAt(new Vector2((goal.x*16),(goal.y*16)))
		Vector2 positionAdjustment = ((Node2D)GetParent().GetParent()).Position;
		Vector2 goal = (Vector2)path[new Vector2(Mathf.Floor(Position.x/16), Mathf.Floor(Position.y/16))][0];
		LookAt(new Vector2((goal.x*16)+8+ positionAdjustment.x,(goal.y*16)+8+ positionAdjustment.y));
		Position += Vector2.Right.Rotated(Rotation) * delta * speed;
	}
	
	public void SetSpawnInfo(float newHealth, float newArmor, object[] effects)
	{  
		health = newHealth;
		armor = newArmor;
		//apply effects
	}
	
	public void UpdatePath(Dictionary<Vector2, object[]> newPath)
	{  
		path = newPath;
	}
}