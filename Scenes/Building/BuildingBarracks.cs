
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class BuildingBarracks : BuildingBase
{

	public Vector2 rallyPoint;
	public Array soldiers  = new Array(){};
	public Soldier soldierType;
	public int maxSoldiers;
	float deploySpread; //how far units will be from the center of the rally point
	public float deploySpeed  = 1.0f;
	
	public void _Ready()
	{  
		deploySpread = 25.0f;
		$SoldierSpawnTimer.wait_time = deploySpeed;
		$SoldierSpawnTimer.Start();
		maxSoldiers = 3;
		soldierType = GD.Load("res://Scenes/Units/Friendly/Soldier.tscn");
	
	}
	
	public bool CheckAccessoryValid(String type)
	{  
		if(accessories.Has(type))
		{
			return false;
		//later on the accessory array will probs have fixed locations for Things (ie shooter [0], lens [1], etc)
		//checking if the building is valid is probs easier with a hardcoded enum || something like tower textures
		}
		return false;
	
	}
	
	public void DoSelection(bool isBeingSelected)
	{  
		base.DoSelection(isBeingSelected) //this is how you super
	
	}
	
	public void MoveRallyPoint(Vector2 newPoint)
	{  
		rallyPoint = newPoint;
		
	//determines a vector2 point around the rally point for a given soldier, where soldier is its slot in the array
	}
	
	public Vector2 GetPointArroundRally(int soldierArrayPos)
	{   
		if((maxSoldiers == 1))
		{
			return rallyPoint;
		}
		Vector2 point  = new Vector2(0,-deploy_spread);
		var degreesSeparation = (PI*2)/max_soldiers;
		point = rallyPoint + point.Rotated(degreesSeparation*soldier_array_pos);
		return point;
		
	}
	
	public void SpawnSoldier()
	{  
		var soldierAmmount = Len(soldiers);
		if(soldierAmmount < maxSoldiers)
		{
			var soldier = soldierType.Instance();
			$Soldiers.AddChild(soldier)
			soldier.position = $SpawnPoint.position
			soldier.home_point = GetPointArroundRally(soldierAmmount);
			soldiers.Append(soldier);
			if((soldierAmmount+1) == maxSoldiers)
			{
				$SoldierSpawnTimer.Stop()
	
			}
		}
	}
	
	public void _OnSoldierSpawnTimerTimeout()
	{  
		rallyPoint = GetGlobalPosition();
		SpawnSoldier();
		pass ;// Replace with function body.
	
	}
	
	public void OnSoldierKilled(__TYPE soldier)
	{  
		$SoldierSpawnTimer.Start(deploySpeed)
		//and also remove from array
	
	
	}
	
	
	
}