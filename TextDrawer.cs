
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class TextDrawer : Node2D
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	
	public __TYPE font = GD.Load("res://Assets/Fonts/MagicCircleFont1.tres");
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		Update();
		pass ;// Replace with function body.
	
	}
	
	public void _Draw()
	{  
		float numberOfLetter = 16.0f;
		var letterRotationIncrement = (PI*(2/number_of_letter));
		int letterRotation = 0;
		
		foreach(var value in GD.Range(numberOfLetter))
		{
			DrawSetTransform(new Vector2(0,0), letterRotationIncrement*value, new Vector2(1,1));
			DrawChar(font, new Vector2(-7,-108), "m", "", Color.fuchsia);
		
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//func _Process(delta):
	//	pass
	
	
		}
	}
	
	
	
}