
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class CircleDrawer : Node2D
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	public __TYPE font = GD.Load("res://Assets/Fonts/MagicCircleFont1.tres");
	public Color transparentColor = new Color(0,0,0,0.0);
	public bool antiallias = false;
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		position = (GetParent().size)/2;
		Update();
	
	
	}
	
	public void _Draw()
	{  
		
		float bounds = 300.0f;
		Vector2 center = new Vector2(0,0);//Vector2((bounds/2),(bounds/2))
		//draw_rect(new Rect2(new Vector2(-size,-size), new Vector2(size*2,size*2)),Color.chartreuse,true, 1,antiallias)
		DrawArc(center, 50.0, 0, 2*PI, 24, Color.fuchsia, 2, antiallias);
		DrawArc(center, 126.0, 0, 2*PI, 60, Color.fuchsia, 4, antiallias);
		//draw_arc(center, 130.0, 0, 2*PI, 60, Color.blue, 4, antiallias)
		//draw_arc(new Vector2(0,0), 154.0, 0, 2*PI, 60, new Color(0.0,0.0,0.0,0.0), 4, true)
		//draw_char(font, new Vector2(-15,-100), "A", "", Color.fuchsia)
		
		//draw_set_transform(center, letterRotationIncrement*value, new Vector2(1,1))
		DrawArc(center + new Vector2(0,-116), 10, 0, 2*PI, 60, Color.fuchsia, 3, antiallias);
		DrawSetTransform(center, PI*1.3 ,new Vector2(1,1));
		//draw_set_transform(new Vector2(0,0), letterRotationIncrement, new Vector2(1,1))
		DrawArc(center + new Vector2(0,-106), 20, 0, 2*PI, 60, Color.fuchsia, 3, antiallias);
	//	for value in GD.Range(numberOfLetter):
	//		DrawSetTransform(new Vector2(0,0), letterRotationIncrement*value, new Vector2(1,1))
	//		DrawChar(font, new Vector2(-15,-110), "A", "", Color.fuchsia)
	//
	//	for value in GD.Range(numberOfLetter):
	//		DrawSetTransform(center, letterRotationIncrement*value, new Vector2(1,1))
	//		DrawChar(font, new Vector2(-6,-108), "m", "", Color.fuchsia)
		
		
		//rotation = PI/6;
	//	DrawSetTransform(new Vector2(0,0), PI/6, new Vector2(1,1))
	//	DrawChar(font, new Vector2(-15,-100), "B", "", Color.fuchsia)
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	}
	
	public void _Process(__TYPE delta)
	{  
		//rotation += .005;
	
	
	}
	
	
	
}