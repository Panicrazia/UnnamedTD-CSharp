
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class StartUp : Node
{
	 
	public override void _Ready()
	{  
		GetNode("MainMenu/Margins/VBox/Play").Connect("pressed", this, "OnNewGamePressed");
		GetNode("MainMenu/Margins/VBox/Quit").Connect("pressed", this, "OnQuitPressed");
	
	}
	
	public void OnNewGamePressed()
	{  
		GetNode("MainMenu").QueueFree();
		Node gameScene = (Node)(GD.Load<PackedScene>("res://Scenes/GameScene.tscn").Instance());
		AddChild(gameScene);
	}
	
	public void OnQuitPressed()
	{  
		GetTree().Quit();
	}
}