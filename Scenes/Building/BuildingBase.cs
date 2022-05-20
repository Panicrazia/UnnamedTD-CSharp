
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class BuildingBase : Node2D
{
	 
	public Array accessories  = new Array(){};

	public Vector2 gridLocation; //the location this is saved to in the GameScene
	public bool isSelected = false;
	
	public void AddAccessory(String type)
	{  
		var accessory = GD.Load("res://Scenes/Accessory/Accessory"+ type +".tscn").Instance();
		accessories.Append(type);
		$Accessories.AddChild(accessory)
	
	}
	
	public bool CheckAccessoryValid(String type)
	{  
		if(accessories.Has(type))
		{
			return false;
		//later on the accessory array will probs have fixed locations for Things (ie shooter [0], lens [1], etc)
		//checking if the building is valid is probs easier with a hardcoded enum || something like tower textures
		}
		return true;
	
	}
	
	public void _Ready()
	{  
	
	}
	
	public void DoSelection(bool isBeingSelected)
	{  
		isSelected = isBeingSelected;
		Update();
		foreach(var value in $Accessories.GetChildren())
		{
			value.FlagSelection(isBeingSelected);
	
		}
	}
	
	public Array GetSelectionInfo()
	{  
		
		return new Array(){};
	
	
	}
	
	
	
}