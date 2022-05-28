using Godot;
using System.Collections.Generic;
using UntitledTowerDefenceGame.Scenes.Effects;

public class Projectile : Node2D
{
    public int damage = 5;
    public List<IEffect> effects = new List<IEffect>();
    public WeakRef target = null;
    public Vector2 velocity = Vector2.Zero;
    public static float speedIncrement = .04f;
    public float speed = speedIncrement * -5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        if (speed < 1)
        {
            speed += speedIncrement;
        }
        if ((target != null) && (target.GetRef() != null))
        {
            Baddie trueTarget = (Baddie)target.GetRef();
            float distanceToTargetSquared = (trueTarget.GetGlobalTransform().origin - GetGlobalTransform().origin).LengthSquared();
            if (speed < 0)
            {
                velocity *= .99f;
            }
            else
            {
                var prediction = Vector2.Right.Rotated(trueTarget.Rotation) * delta * trueTarget.speed;
                var pull = Mathf.Clamp(1000 / distanceToTargetSquared, 0, 1);
                velocity *= (1 - pull);
                velocity += ((trueTarget.GetGlobalTransform().origin + prediction - GetGlobalTransform().origin)).Normalized() * 800 * speed * pull;
            }
            //check if target should be hit
            if (distanceToTargetSquared < 100) //should probs later be dependant on the enemy how close counts for hitting
            {
                trueTarget.TakeDamage(damage);
                trueTarget.TakeEffects(effects);
                QueueFree();
            }
        }
        else
        {
            target = ((AccessoryOrb)GetParent().GetParent()).GetTarget();
            //ask parent for a new target or do whatever idk im not your boss
        }
        Position += velocity * delta;
    }

    public void OnProjectileAreaEntered(Area2D area)
    {
        //can be used to determine booster rings and whatnot
    }

    public void SetTarget(Baddie targetToSet)
    {
        target = WeakRef(targetToSet);
    }
}