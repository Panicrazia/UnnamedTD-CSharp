
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class baddie : Node2D
{
	 
	public const int CELLSize = 16;
	//health && whatnot should be changed to consts to have the base health/armor/whatnot
	public int health = 10;
	public int armor = 0;
	public int speed = 25;
	public Array effects  = new Array(){};
	public __TYPE velocity = Vector2.ZERO;
	public Dictionary path
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		
	}
	
	public void Damage(__TYPE damageDone)
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
	
	public void DoEffects(Array effects)
	{  
		//print("baddie got hit by a projectle")
	
	}
	
	public void _PhysicsProcess(__TYPE delta)
	{  
		//probs should have a thing where the goal location is updated when the path changes || they enter a new tile, to avoid many maths
		if((position))
		{
	//		var adjustedPosition = position + GetParent().GetParent().position;
	//		var goal = path[new Vector2(Mathf.Floor(adjustedPosition.x/16 as int), Mathf.Floor(adjustedPosition.y/16 as int))];
	//		LookAt(new Vector2((goal.x*16),(goal.y*16)))
			var positionAdjustment = GetParent().GetParent().position;
			var goal = path[new Vector2(Mathf.Floor(position.x/16 as int), Mathf.Floor(position.y/16 as int))][0];
			LookAt(new Vector2((goal.x*16)+8+position_adjustment.x,(goal.y*16)+8+position_adjustment.y));
			position += Vector2.RIGHT.Rotated(rotation) * delta * speed;
	
	
		}
	}
	
	public void SetSpawnInfo(__TYPE newHealth, __TYPE newArmor, Array effects)
	{  
		health = newHealth;
		armor = newArmor;
		//apply effects
	
	}
	
	public void UpdatePath(Dictionary newPath)
	{  
		path = newPath;
	
	
	}
	
	
	
}