
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class projectile : Node2D
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	public int damage = 5;
	public Array effects = new Array(){};
	public __TYPE target = null;
	public __TYPE velocity = Vector2.ZERO;
	public float speedIncrement = .04f;
	public __TYPE speed = speedIncrement * -5;
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		pass ;// Replace with function body.
	
	
	}
	
	public void _PhysicsProcess(__TYPE delta)
	{  
		if(speed < 1)
		{
				speed += speedIncrement;
		}
		if((target != null) && (target.GetRef()))
		{
			if(speed < 0)
			{
				velocity*=.99
			}
			else
			{
				var prediction = Vector2.RIGHT.Rotated(target.GetRef().GetParent().rotation) * delta * target.GetRef().GetParent().speed;
				var pull = Mathf.Clamp(1000/(target.GetRef().GetGlobalTransform().origin - GetGlobalTransform().origin).LengthSquared(), 0, 1);
				velocity *= (1-pull)
				velocity += ((target.GetRef().GetGlobalTransform().origin + prediction - GetGlobalTransform().origin)).Normalized()*800*speed*pull;
			}
		}
		else
		{
			target = GetParent().GetParent().GetTarget();
			//ask parent for a new target || do whatever idk im !your boss
		}
		position += velocity * delta;
	
	//projectiles should look at their parent for a target, since the tower will be their daddy && will always have what its currently targeting
	
	}
	
	public void _OnProjectileAreaEntered(__TYPE area)
	{  
		if(area.IsInGroup("baddies"))
		{
			if(((target != null) && (area == target.GetRef())))
			{
				area.GetParent().Damage(damage);
				area.GetParent().DoEffects(effects);
				QueueFree();
			
	
			}
		}
	}
	
	public void SetTarget(__TYPE targetToSet)
	{  
		target = Weakref(targetToSet);
	
	
	}
	
	
	
}