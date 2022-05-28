
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using UntitledTowerDefenceGame.Scenes.Effects;
using UntitledTowerDefenceGame.Scenes.Units;

public class Soldier : Node2D, IBattler
{
	//Have to add an IBattler interface for freindlies and enemies
	public Vector2 homePoint;

	public List<Baddie> enemyArray = new List<Baddie>();
	public float combatRange;
	public float engageRange;
	public int attackCooldown  = 120;
	public int currentAttackCooldown  = 0;
	//shot timers && range areas should probably be added via code to make new accessories !a massive pain inthe balls

	public const int CELLSize = 16;
	public float health = 10;
	public float armor = 0;
	public float speed = 45;
	public float damage = 1;
	public bool inCombat = false;
	public bool isAlive = true;
	public int framesDead = 0;
	public IBattler target = null;
	public List<IEffect> currentEffects = new List<IEffect>();


	public float Health { get => health; set => health = value; }
	public float Armor { get => armor; set => armor = value; }
	public float Speed { get => speed; set => speed = value; }
	public float Damage { get => damage; set => damage = value; }
	public int AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
	public int CurrentAttackCooldown { get => currentAttackCooldown; set => currentAttackCooldown = value; }
	public bool InCombat { get => inCombat; set => inCombat = value; }
	public bool IsAlive { get => isAlive; set => isAlive = value; }
	public IBattler Target { get => target; set => target = value; }
	public List<IEffect> CurrentEffects { get => currentEffects; set => currentEffects = value; }

	public override void _Ready()
	{
		combatRange = ((CircleShape2D)((CollisionShape2D)GetNode("HitBox/HitboxRange")).Shape).Radius;
		engageRange = ((CircleShape2D)((CollisionShape2D)GetNode("EnemyDecection/EngageRange")).Shape).Radius + 20;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (isAlive)
		{
			if (target != null)
			{
				if (target.IsAlive) //attack
				{
					//check if targets target is this solider, if not then pick new target, then
					
					if ((MoveTowardsTarget(delta)))
					{
                        if (InstigateTarget() || target.Target.Equals(this))
                        {
							if ((currentAttackCooldown <= 0))
							{
								AttackTarget();
							}
						}
                        else
                        {
                            if (!SelectTarget()) //right now these guys select a target every single physicis phase so if I can optimize this then that would be good
                            {
								if ((currentAttackCooldown <= 0))
								{
									AttackTarget();
								}
							}
						}
					}
				}
				else
				{
					target = null;
					SelectTarget();
				}
			}
			else //move towards home
			{
				inCombat = false;
				SelectTarget();
				if ((GlobalPosition.DistanceSquaredTo(homePoint) > 1))
				{
					LookAt(homePoint);
					Position += Vector2.Right.Rotated(Rotation) * delta * speed;
				}
			}
		}
		else
		{
			framesDead++;
			Visible = false;
			if (framesDead > 10) //This should connect to a global value for corpses dissapearing, that varies with how laggy it probably is
			{
				QueueFree();
			}
		}
		((Sprite)GetNode("Sprite")).Rotation = -Rotation;
		currentAttackCooldown -= 1;
	}
	
	public bool MoveTowardsTarget(float delta)
	{   //returns true if already in range
		Vector2 targetPosition = ((Baddie)target).GetGlobalTransform().origin;
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
			if (target != null && target.Equals(trueBaddie))
			{
				target = null;
				SelectTarget();
			}
		}
	}
	
	/* returns true if target was switched */
	public bool SelectTarget()
	{  
		//friendly units should probably only target Closest (with exceptions for certain units), currently targets first in list
		if(enemyArray.Count > 0)
		{
			List<Baddie> listToCheck = enemyArray;
			List<Baddie> nonBattlingEnemies = enemyArray.FindAll(IsBaddieNotInCombat);
			if(nonBattlingEnemies.Count >= 1)
            {
				listToCheck = nonBattlingEnemies;
			}
			foreach (Baddie enemy in listToCheck)
			{
				Vector2 targetPosition = enemy.GetGlobalTransform().origin;
				bool isWithinRange = (engageRange*engageRange) > (targetPosition.DistanceSquaredTo(homePoint));
				bool flag = true;
				if(isWithinRange)
				{
                    if (enemy.Equals(target)) //if you were already targetting the enemy
                    {
						flag = false;
					}
					EnterCombat(enemy);
					return flag;
				}
			}
		}
		return false;
	}

	public bool IsBaddieNotInCombat(Baddie baddie)
    {
		return (baddie.isAlive && !baddie.inCombat);
	}

	/* returns true if a target was selected, false otherwise */
	public bool EnterCombat(IBattler newTarget)
	{
		if (newTarget.IsAlive)
		{
			Target = newTarget;
			inCombat = true;
			return true;
		}
		return false;
	}

	public void LeaveCombat()
	{
		Target = null;
		inCombat = false;
	}

	public bool AttackTarget()
	{
		currentAttackCooldown = attackCooldown;
		return Target.TakeDamage(damage, this);
	}

	/* returns true if target's target was switched to this unit */
	public bool InstigateTarget()
	{
		if (target.Target == null || !target.Target.IsAlive)
		{
			return target.EnterCombat(this);
		}
		return false;
	}

	/* returns true if damage was taken, false otherwise */
	public bool TakeDamage(float damageDone)
	{
		damageDone -= armor;
		if (damageDone > 0)
		{
			health -= damageDone;
			if (health <= 0)
			{
				isAlive = false;
			}
			return true;
		}
		return false;
	}

	/* returns true if target was switched/attacked back */
	public bool TakeDamage(float damageDone, IBattler attacker)
	{
		TakeDamage(damageDone);
		/*
		if (Target == null || !Target.IsAlive)
		{
			return EnterCombat(attacker);
		}
		*/
		return false;
	}

	public void TakeEffects(List<IEffect> effects)
	{
		foreach (IEffect effect in effects)
		{
			currentEffects.Add(effect);
		}
	}
}