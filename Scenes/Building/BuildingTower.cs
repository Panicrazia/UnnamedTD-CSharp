
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class BuildingTower : BuildingBase
{
	 
	public new bool CheckAccessoryValid(String type)
	{  
		if(accessories.Has(type))
		{
			return false;
		//later on the accessory array will probs have fixed locations for Things (ie shooter [0], lens [1], etc)
		//checking if the building is valid is probs easier with a hardcoded enum || something like tower textures
		}
		return true;
	
	
	}
	
	
	
}