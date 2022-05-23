
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class Soldier : Node2D
{
	//Have to add an IBattler interface for freindlies and enemies
	public Vector2 homePoint;
	public float speed  = 45.0f;
	public int damage = 100;
	public object[] effects = new object[0];

	public List<Baddie> enemyArray = new List<Baddie>();
	public WeakRef currentTarget = null;
	public bool inCombat  = false;
	public float combatRange;
	public float engageRange;
	public int attackCooldown  = 120;
	public int currentAttackCooldown  = 0;
	//shot timers && range areas should probably be added via code to make new accessories !a massive pain inthe balls
	
	public override void _Ready()
	{
		combatRange = ((CircleShape2D)((CollisionShape2D)GetNode("HitBox/HitboxRange")).Shape).Radius;
		engageRange = ((CircleShape2D)((CollisionShape2D)GetNode("EnemyDecection/EngageRange")).Shape).Radius + 20;
	}

	public override void _PhysicsProcess(float delta)
	{  
		if((inCombat && (currentTarget != null)))
		{
			if(currentTarget.GetRef() == null) //if enemy is dead
			{
				inCombat = false;
				SelectTarget();
			}
			else
			{
				if((MoveTowardsTarget(delta)))
				{
					if((currentAttackCooldown <= 0))
					{
						AttackTarget();
					
					}
				}
			}
		}
		else
		{
			SelectTarget();
			if((GlobalPosition.DistanceSquaredTo(homePoint) > 1))
			{
				LookAt(homePoint);
				Position += Vector2.Right.Rotated(Rotation) * delta * speed;
			}
		}
		((Sprite)GetNode("Sprite")).Rotation = -Rotation;
		currentAttackCooldown -= 1;
	}
	
	public void AttackTarget()
	{  
		currentAttackCooldown = attackCooldown;
		Baddie actualTarget = (Baddie)currentTarget.GetRef();
		actualTarget.Damage(damage);
		actualTarget.DoEffects(effects);
	}
	
	public bool MoveTowardsTarget(float delta)
	{   //returns true if already in range
		Vector2 targetPosition = ((Baddie)currentTarget.GetRef()).GetGlobalTransform().origin;
		bool isWithinRange = (combatRange* combatRange) > (targetPosition - GetGlobalTransform().origin).LengthSquared();
		if(!isWithinRange)
		{
			LookAt(targetPosition);
			Position += Vector2.Right.Rotated(Rotation) * delta * speed;
		}
		return isWithinRange;
	
	}
	
	public void OnEnemyDecectionAreaEntered(Area2D area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Add((Baddie)area.GetParent());
		}
	}
	
	public void OnEnemyDecectionAreaExited(Area2D area)
	{  
		if (area.IsInGroup("baddies"))
		{
			Baddie trueBaddie = (Baddie)area.GetParent();
			enemyArray.Remove(trueBaddie);
			if (currentTarget.GetRef() == trueBaddie)
			{
				currentTarget = null;
				SelectTarget();
			}
		}
	}
	
	public void SelectTarget()
	{  
		//friendly units should probably only target Closest (with exceptions for certain units)
		if(enemyArray.Count > 0)
		{
			foreach(var enemy in enemyArray)
			{
				var targetPosition = enemy.GetGlobalTransform().origin;
				var isWithinRange = (engageRange*engageRange) > (targetPosition.DistanceSquaredTo(homePoint));
				if(isWithinRange)
				{
					currentTarget = WeakRef(enemy);
					break;
	//	for target in enemyArray:
	//		#currently a very scuffed targetting system
	//		currentTarget = Weakref(target);
				}
			}
		}
		if(currentTarget != null)
		{
			inCombat = true;	
		}
	}
	
	public WeakRef GetTarget()
	{  
		return currentTarget;
	}
}