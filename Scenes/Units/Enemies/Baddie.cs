
using Godot;
using System;
using System.Collections.Generic;
using UntitledTowerDefenceGame.Scenes.Effects;
using UntitledTowerDefenceGame.Scenes.Units;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public class Baddie : Node2D, IBattler
{
    //TODO: make an enemy handler in c++ so the game will slow to a pathetic crawl at the slowest rate possible
    public const int CELLSize = 16;
    public float health = 10;
    public float armor = 0;
    public float speed = 25;
    public float damage = 1;
    public int attackCooldown = 120;
    public int currentAttackCooldown = 0;
    public bool inCombat = false;
    public bool isAlive = true;
    public int framesDead = 0;
    public IBattler target = null;
    public List<IEffect> currentEffects = new List<IEffect>();
    public Vector2 velocity = Vector2.Zero;
    public Dictionary<Vector2, object[]> path;
    public Vector2 lastValidGoal;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public virtual void DoEffects()
    {
        //go through the effects list and do them
    }

    public override void _PhysicsProcess(float delta)
    {
        if (isAlive)
        {
            if(target != null) 
            {
                if (target.IsAlive) //attack
                {
                    if ((MoveTowardsTarget(delta)))
                    {
                        InstigateTarget();
                        if ((currentAttackCooldown <= 0))
                        {
                            AttackTarget();
                        }
                    }
                }
                else
                {
                    target = null;
                }
            }
            else //move towards goal
            {
                inCombat = false;
                Vector2 positionAdjustment = ((Node2D)GetParent().GetParent()).Position;
                Vector2 goal;
                if (path.TryGetValue(new Vector2(Mathf.Floor(Position.x / 16), Mathf.Floor(Position.y / 16)), out object[] potentialGoal))
                {
                    goal = (Vector2)potentialGoal[0];
                    lastValidGoal = goal;
                }
                else
                {
                    //TODO: make it so if you build on them then they just default to the base map for pathfinding,
                    //currently this implimentation breaks by them just sticking in one spot if their last goal is also invalid
                    goal = lastValidGoal;
                }
                
                //Vector2 goal = (Vector2)path[new Vector2((int)(Position.x / 16), (int)(Position.y / 16))][0];
                LookAt(new Vector2((goal.x * 16) + 8 + positionAdjustment.x, (goal.y * 16) + 8 + positionAdjustment.y));
                Position += Vector2.Right.Rotated(Rotation) * delta * speed;
                //then check for a target
            }
        }
        else
        {
            framesDead++;
            Visible = false;
            if(framesDead > 10) //This should connect to a global value for corpses dissapearing, that varies with how laggy it probably is
            {
                QueueFree();
            }
        }
        ((Sprite)GetNode("Sprite")).Rotation = -Rotation;
        currentAttackCooldown -= 1;
    }

    public bool MoveTowardsTarget(float delta)
    {   //returns true if already in range
        Vector2 targetPosition = ((Soldier)target).GetGlobalTransform().origin;
        //TODO: switch out that 100 literal with an actual combat range
        bool isWithinRange = (100) > (targetPosition - GetGlobalTransform().origin).LengthSquared();
        if (!isWithinRange)
        {
            LookAt(targetPosition);
            Position += Vector2.Right.Rotated(Rotation) * delta * speed;
        }
        return isWithinRange;
    }

    public virtual void SetSpawnInfo(float healthMultiplier, float armorMultiplier, object[] effects)
    {
        health *= healthMultiplier;
        GD.Print("Baddie spawned with " + health + " health from a multiplier of " + healthMultiplier);
        armor *= armorMultiplier;
        //apply effects
    }

    public void UpdatePath(Dictionary<Vector2, object[]> newPath)
    {
        path = newPath;
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
                //to add death animations this will need to be optional, and cannot rely on weakrefs alone to handle targetting
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
        if(Target == null || !Target.IsAlive)
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