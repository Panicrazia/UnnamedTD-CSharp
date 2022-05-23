
using Godot;
using System;
using System.Collections.Generic;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;

public class Baddie : Node2D
{
    //TODO: make an enemy handler in c++ so the game will slow to a pathetic crawl at the slowest rate possible
    public const int CELLSize = 16;
    public float health = 10;
    public float armor = 0;
    public float speed = 25;
    public object[] effects = new object[0];
    public Vector2 velocity = Vector2.Zero;
    public Dictionary<Vector2, object[]> path;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public virtual void Damage(float damageDone)
    {
        damageDone -= armor;
        if (damageDone > 0)
        {
            health -= damageDone;
            if (health < 0)
            {
                //to add death animations this will need to be optional, and cannot rely on weakrefs alone to handle targetting
                QueueFree();
            }
        }
    }

    public virtual void DoEffects(object[] effects)
    {
        //print("Baddie got hit by a projectle")
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 positionAdjustment = ((Node2D)GetParent().GetParent()).Position;
        Vector2 goal = (Vector2)path[new Vector2(Mathf.Floor(Position.x / 16), Mathf.Floor(Position.y / 16))][0];
        //Vector2 goal = (Vector2)path[new Vector2((int)(Position.x / 16), (int)(Position.y / 16))][0];
        LookAt(new Vector2((goal.x * 16) + 8 + positionAdjustment.x, (goal.y * 16) + 8 + positionAdjustment.y));
        Position += Vector2.Right.Rotated(Rotation) * delta * speed;
    }

    public virtual void SetSpawnInfo(float healthMultiplier, float armorMultiplier, object[] effects)
    {
        health *= healthMultiplier;
        GD.Print("Baddie spawned with " + health + "health");
        armor *= armorMultiplier;
        //apply effects
    }

    public void UpdatePath(Dictionary<Vector2, object[]> newPath)
    {
        path = newPath;
    }
}