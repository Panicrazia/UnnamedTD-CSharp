
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using System.Collections.Generic;
using UntitledTowerDefenceGame.Scenes.Units;

public class AccessoryOrb : AccessoryBase, IShooter
{
	 
	public List<Baddie> enemyArray = new List<Baddie>();
	public IBattler target;
	public PackedScene projectile = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");

	public int attackRange = 150;
	public int attackSpeed = 0;
	public int instablility = 200;
	public bool selected = false;

    public PackedScene Projectile { get => projectile; set => projectile = value; }

    //shot timers && range areas should probably be added via code to make new accessories not a massive pain inthe balls

    public override void _Ready()
	{
        CircleShape2D shape = new CircleShape2D
        {
            Radius = attackRange
        };
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
		if(target == null || !target.IsAlive)
		{
			SelectTarget();
		}
		Rotation += .1f; //maybe only the sprite should spin?
		((YSort)GetNode("Projectiles")).Rotation = (-Rotation);
		
	}
	
	public void FlagSelection(bool isBeingSelected)
	{  
		selected = isBeingSelected;
		Update();
		
	}
	
	public bool SelectTarget()
	{
		((Timer)GetNode("ShotTimer")).Paused = true;
		//friendly units should probably only target Closest (with exceptions for certain units), currently targets first in list
		if (enemyArray.Count > 0)
		{
			List<Baddie> eligibleBaddies = enemyArray.FindAll(IsBaddieAlive);
			if (eligibleBaddies.Count >= 1)
			{
				target = eligibleBaddies[0];
			}
		}
		if (target != null)
		{
			((Timer)GetNode("ShotTimer")).Paused = false;
			return true;
		}
		return false;
	}

	public bool IsBaddieAlive(Baddie baddie)
	{
		return baddie.isAlive;
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
			if(target.Equals(trueBaddie))
			{
				target = null;
				SelectTarget();
			}
		}
	}

	public IBattler GetTarget()
	{  
		return target;
	
	}
	
	public void OnShotTimerTimeout()
	{
		ShootProjectile();
	}

    public bool ShootProjectile()
    {
		if (target != null && target.IsAlive)
		{
			Projectile instance = (Projectile)projectile.Instance();
			instance.SetTarget(target);
			//this Projectile spawn will be somewhere different when I get full tower customization done
			instance.Position = ((Node2D)GetNode("ProjectileSpawn")).Position;
			instance.velocity = new Vector2(((GD.Randf() * 2) - 1), ((GD.Randf() * 2) - 1)).Normalized() * instablility;
			//.GetGlobalTransform() would go before origin except that these are children of the towers
			((YSort)GetNode("Projectiles")).AddChild(instance);
			return true;
		}
        else
        {
			SelectTarget();
        }
		return false;
	}
}