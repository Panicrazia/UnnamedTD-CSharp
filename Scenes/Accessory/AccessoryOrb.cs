
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using System.Collections.Generic;

public class AccessoryOrb : Node2D
{
	 
	public List<Baddie> enemyArray = new List<Baddie>();
	public WeakRef currentTarget = WeakRef(null);
	public PackedScene shot = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");

	public int attackRange = 150;
	public int attackSpeed = 0;
	public int instablility = 200;
	public bool selected = false;
	//shot timers && range areas should probably be added via code to make new accessories not a massive pain inthe balls

	public override void _Ready()
	{
		CircleShape2D shape = new CircleShape2D();
		shape.Radius = attackRange;
		((CollisionShape2D)GetNode("Range/CollisionShape2D")).Shape = shape;
		((Timer)GetNode("ShotTimer")).Start(); //this timer might need to be replaced with something else due to how it doenst work for very small Values (waittime < .05 sec)
		((Timer)GetNode("ShotTimer")).Paused = true;
		((YSort)GetNode("Projectiles")).Modulate = Color.ColorN("Red");
	
	}
	
	public override void _Draw()
	{  
		if(selected)
		{
			DrawCircle(new Vector2(0,0), attackRange, new Color(0, 1, 1, .25f));
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{  
		if(currentTarget == null)
		{
			SelectTarget();
		}
		else
		{
			if(currentTarget.GetRef() == null)
			{
				currentTarget = null;
				SelectTarget();
			}
			else
			{
				//look_at(currentTarget.GetRef())
			}
		}
		Rotation += .1f;
		((YSort)GetNode("Projectiles")).Rotation = (-Rotation);
		
	}
	
	public void FlagSelection(bool isBeingSelected)
	{  
		selected = isBeingSelected;
		Update();
		
	}
	
	public void SelectTarget()
	{
		((Timer)GetNode("ShotTimer")).Paused = true;
		//target First (there are cases where this will !target the frontmost enemy, such as changing their paths)
		if (enemyArray.Count > 0)
		{
			currentTarget = WeakRef(enemyArray[0]);
		}
		if(currentTarget != null)
		{
			((Timer)GetNode("ShotTimer")).Paused = false;
		}
	}
	
	public void OnAttackRangeAreaEntered(Area2D area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Add((Baddie)area.GetParent());
	
	
		}
	}
	
	public void OnAttackRangeAreaExited(Area2D area)
	{  
		if(area.IsInGroup("baddies"))
		{
			Baddie trueBaddie = (Baddie)area.GetParent();
            enemyArray.Remove(trueBaddie);
			if(currentTarget.GetRef() == trueBaddie)
			{
				currentTarget = null;
				SelectTarget();
			}
		}
	}
	
	public WeakRef GetTarget()
	{  
		return currentTarget;
	
	}
	
	public void OnShotTimerTimeout()
	{  
		if(currentTarget.GetRef() != null)
		{
			Projectile instance = (Projectile)shot.Instance();
			instance.SetTarget((Baddie)currentTarget.GetRef());
			//this Projectile spawn will be somewhere different when I get full tower customization done
			instance.Position = ((Node2D)GetNode("ProjectileSpawn")).Position;
			instance.velocity = new Vector2(((GD.Randf()*2)-1),((GD.Randf()*2)-1)).Normalized()*instablility;
			//.GetGlobalTransform() would go before origin except that these are children of the towers
			((YSort)GetNode("Projectiles")).AddChild(instance);
	
		}
	}
	
	
	
}