
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class BuildingBase : Node2D
{
	 
	public List<String> accessories  = new List<String>();

	public Vector2 gridLocation; //the location this is saved to in the GameScene
	public bool isSelected = false;
	
	public virtual void AddAccessory(String type)
	{
		accessories.Add(type);
		var accessory = GD.Load<PackedScene>("res://Scenes/Accessory/Accessory"+ type +".tscn").Instance();
		((YSort)GetNode("Accessories")).AddChild(accessory);
	}
	
	public virtual bool CheckAccessoryValid(String type)
	{  
		if(accessories.Contains(type))
		{
			return false;
		//later on the accessory array will probs have fixed locations for Things (ie shooter [0], lens [1], etc)
		//checking if the building is valid is probs easier with a hardcoded enum || something like tower textures
		}
		return true;
	}
	
	public virtual void DoSelection(bool isBeingSelected)
	{  
		isSelected = isBeingSelected;
		Update();
		foreach(AccessoryOrb value in ((YSort)GetNode("Accessories")).GetChildren())
		{
			value.FlagSelection(isBeingSelected);
	
		}
	}
	
	public virtual object[] GetSelectionInfo()
	{  
		return new object[3] { 6, Color.ColorN("Blue"), new Vector2(.3f, .3f) };
	}
}