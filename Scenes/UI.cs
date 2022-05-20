
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class UI : CanvasLayer
{
	 
	// Declare member variables here. Examples:
	// int a = 2;
	// string b = "text";
	public Color colorDeny = new Color(1,0,0,.5);
	public Color colorOkay = new Color(0,1,0,.5);
	
	
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		pass ;// Replace with function body.
	
	
	//building preveiws will need to change back to just sprites so you can build while paused
	//func SetBuildingPreview(type, pos):
	//	var dragTower = GD.Load("res://Scenes/Building" + type + ".tscn").Instance();
	//	dragTower.name = "DragTower";
	//	dragTower.modulate = colorDeny;
	//
	//	var control = new Control()
	//	control.AddChild(dragTower, true)
	//	control.rect_position = pos;
	//	control.name = "BuildingPreview";
	//	AddChild(control, true)
	//	MoveChild(GetNode("BuildingPreview"), 0)
	//
	//func UpdateBuildingPreview(pos, color):
	//	GetNode("BuildingPreview").rect_position = pos;
	//	if (GetNode("BuildingPreview/DragTower").modulate != color):
	//		GetNode("BuildingPreview/DragTower").modulate = color;
	//	pass
	
	
	
	}
	
	
	
}