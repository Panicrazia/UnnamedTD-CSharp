
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class MagicCircleAttempt2 : Node2D
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	
	
	// Called when the node enters the scene tree for the first time.
	public async void _Ready()
	{  
		//MAKE SURE ALL THE POINTS OF ROTATION AND SIZE BOUNDS GET SET CORRECTLY HERE
		
		$TextureRect.rect_position = $TextureRect.rect_size/-2
		$TextureRect/TextDrawer.position = $TextureRect.rect_size/2
		//scale = new Vector2(.5,.5);
		$Control.rect_global_position = new Vector2(-1000,-1000);
		//modulate.a = .7;
	//	await ToSignal(VisualServer, "frame_post_draw")
	//	$Control.Hide()
		pass ;// Replace with function body.
	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	}
	
	public void _Process(__TYPE delta)
	{  
		$TextureRect.rect_rotation += .3;
		//rotation += .003;
		//$Control.rect_rotation = -rotation
	
	
	}
	
	
	
}