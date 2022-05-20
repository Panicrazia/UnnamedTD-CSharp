
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class AccessoryOrb : Node2D
{
	 
	public Array enemyArray = new Array(){};
	public __TYPE currentTarget = null;
	public __TYPE instance;
	public int attackRange = 150;
	public int attackSpeed = 0;
	public __TYPE shot = GD.Load("res://Scenes/projectile.tscn");
	public bool selected = false;
	public int instablility = 200;
	//shot timers && range areas should probably be added via code to make new accessories !a massive pain inthe balls
	
	public void _Ready()
	{  
		var shape = new CircleShape2D()
		shape.SetRadius(attackRange);
		$Range/CollisionShape2D.shape = shape;
		$ShotTimer.Start() //this timer might need to be replaced with something else due to how it doenst work for very small Values (waittime < .05 sec)
		$ShotTimer.SetPaused(true)
		$Projectiles.modulate = Color.red;
	
	}
	
	public void _Draw()
	{  
		if(selected)
		{
			DrawCircle(new Vector2(0,0), attackRange, new Color(0, 1, 1, .25));
		
	// Called every frame. 'delta' is the elapsed time since the previous frame.
		}
	}
	
	public void _PhysicsProcess(__TYPE delta)
	{  
		if(!current_target)
		{
			SelectTarget();
		}
		else
		{
			if((!current_target.GetRef()))
			{
				currentTarget = null;
				$ShotTimer.SetPaused(true)
				SelectTarget();
			}
			else
			{
				//look_at(currentTarget.GetRef())
			}
		}
		rotation += .1;
		$Projectiles.rotation = (-rotation)
		
	}
	
	public void FlagSelection(bool isBeingSelected)
	{  
		selected = isBeingSelected;
		Update();
		
	}
	
	public void SelectTarget()
	{  
		//target First (there are cases where this will !target the frontmost enemy, such as changing their paths)
		if(enemyArray.Size() > 0)
		{
			currentTarget = Weakref(enemyArray[0]);
	//	for target in enemyArray:
	//		#currently a very scuffed targetting system
	//		currentTarget = Weakref(target);
		}
		if(currentTarget)
		{
			$ShotTimer.SetPaused(false)
	
	
		}
	}
	
	public void _OnAttackRangeAreaEntered(__TYPE area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Append(area);
	
	
		}
	}
	
	public void _OnAttackRangeAreaExited(__TYPE area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Erase(area);
			if(currentTarget.GetRef() && area == currentTarget.GetRef())
			{
				currentTarget = null;
				$ShotTimer.SetPaused(true)
				SelectTarget();
	
			}
		}
	}
	
	public __TYPE GetTarget()
	{  
		return currentTarget;
	
	}
	
	public void _OnShotTimerTimeout()
	{  
		if(currentTarget.GetRef())
		{
			instance = shot.Instance();
			instance.SetTarget(currentTarget.GetRef());
			//this projectile spawn will be somewhere different when I get full tower customization done
			instance.position = $ProjectileSpawn.position
			instance.velocity = new Vector2(((GD.Randf()*2)-1),((GD.Randf()*2)-1)).Normalized()*instablility;
			//.GetGlobalTransform() would go before origin except that these are children of the towers
			$Projectiles.AddChild(instance)
	
	
		}
	}
	
	
	
}