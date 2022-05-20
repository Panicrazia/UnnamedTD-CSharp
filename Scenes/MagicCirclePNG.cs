
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class MagicCirclePNG : Node2D
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	public string circle  = "res://Assets/Magical Circles/Circle6.png";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{  
		//$Projectiles.modulate = Color.red;
		//$PNGSprite.modulate = new Color(1,0,1,1);
		//$PNGSprite.texture = GD.Load(circle);
		//activate()
	
	}
	
	public override void _Process(float delta)
	{  
		Rotation += .005f;
	}
	
	public void Activate()
	{  
		Visible = true;
	
	}
	
	public void SetCircle(int circleId)
	{  
		circle = "res://Assets/Magical Circles/Circle" + circleId + ".png";
	
	}
	
	public void SetColor(Color newColor)
	{
		GetNode<Sprite>("PNGsprite").Modulate = newColor;
	}
}