
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class StartUp : Node
{
	 
	public void _Ready()
	{  
		GetNode("MainMenu/Margins/VBox/Play").Connect("pressed", this, "on_new_game_pressed");
		GetNode("MainMenu/Margins/VBox/Quit").Connect("pressed", this, "on_quit_pressed");
	
	}
	
	public void OnNewGamePressed()
	{  
		GetNode("MainMenu").QueueFree();
		var gameScene = GD.Load("res://Scenes/GameScene.tscn").Instance();
		AddChild(gameScene);
		
	}
	
	public void OnQuitPressed()
	{  
		GetTree().Quit();
	
	
	}
	
	
	
}