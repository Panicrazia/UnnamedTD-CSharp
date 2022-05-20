
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class Soldier : Node2D
{
	 
	public Vector2 homePoint
	public float speed  = 45.0f;
	public int damage = 100;
	public Array effects = new Array(){};
	
	public Array enemyArray = new Array(){};
	public __TYPE currentTarget = null;
	public bool inCombat  = false;
	public int combatRange
	public int engageRange
	public int attackCooldown  = 120;
	public int currentAttackCooldown  = 0;
	//shot timers && range areas should probably be added via code to make new accessories !a massive pain inthe balls
	
	public void _Ready()
	{  
		combatRange = $HitBox/HitboxRange.shape.radius
		engageRange = $EnemyDecection/EngageRange.shape.radius + 20
	
	}
	
	public void _PhysicsProcess(__TYPE delta)
	{  
		if((inCombat && currentTarget))
		{
			if((!current_target.GetRef())) //if enemy is dead
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
			if((!enemy_array.Empty()))
			{
				SelectTarget();
			}
			if((globalPosition.DistanceSquaredTo(homePoint) > 1))
			{
				LookAt(homePoint);
				position += Vector2.RIGHT.Rotated(rotation) * delta * speed;
			}
		}
		$Sprite.rotation = -rotation
		currentAttackCooldown -= 1;
	
	}
	
	public void AttackTarget()
	{  
		currentAttackCooldown = attackCooldown;
		var actualTarget = currentTarget.GetRef().GetParent();
		actualTarget.Damage(damage);
		actualTarget.DoEffects(effects);
	
	}
	
	public bool MoveTowardsTarget(__TYPE delta)
	{   //returns true if already in range
		var targetPosition = currentTarget.GetRef().GetGlobalTransform().origin;
		var isWithinRange = (combatRange*combat_range) > (targetPosition - GetGlobalTransform().origin).LengthSquared();
		if((!is_within_range))
		{
			LookAt(targetPosition);
			position += Vector2.RIGHT.Rotated(rotation) * delta * speed;
		}
		return isWithinRange;
	
	}
	
	public void _OnEnemyDecectionAreaEntered(__TYPE area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Append(area);
	
		}
	}
	
	public void _OnEnemyDecectionAreaExited(__TYPE area)
	{  
		if(area.IsInGroup("baddies"))
		{
			enemyArray.Erase(area);
			if((currentTarget && currentTarget.GetRef() && area == currentTarget.GetRef()))
			{
				currentTarget = null;
				SelectTarget();
	
			}
		}
	}
	
	public void SelectTarget()
	{  
		//friendly units should probably only target Closest (with exceptions for certain units)
		if(enemyArray.Size() > 0)
		{
			foreach(var enemy in enemyArray)
			{
				var targetPosition = enemy.GetGlobalTransform().origin;
				var isWithinRange = (engageRange*engage_range) > (targetPosition.DistanceSquaredTo(homePoint));
				if(isWithinRange)
				{
					currentTarget = Weakref(enemy);
					break;
	//	for target in enemyArray:
	//		#currently a very scuffed targetting system
	//		currentTarget = Weakref(target);
				}
			}
		}
		if(currentTarget)
		{
			inCombat = true;
	
		}
	}
	
	public __TYPE GetTarget()
	{  
		return currentTarget;
	
	
	}
	
	
	
}