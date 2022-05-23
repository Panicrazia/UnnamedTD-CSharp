
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class BuildingBarracks : BuildingBase
{

	public Vector2 rallyPoint;
	public List<Soldier> soldiers  = new List<Soldier>();
	public PackedScene soldierType;
	public int maxSoldiers;
	float deploySpread; //how far units will be from the center of the rally point
	public float deploySpeed  = 1.0f;
	
	public override void _Ready()
	{  
		deploySpread = 25.0f;
		((Timer)GetNode("SoldierSpawnTimer")).WaitTime = deploySpeed;
		((Timer)GetNode("SoldierSpawnTimer")).Start();
		maxSoldiers = 3;
		soldierType = GD.Load<PackedScene>("res://Scenes/Units/Friendly/Soldier.tscn");
	}
	
	public override bool CheckAccessoryValid(String type)
	{  
		if(accessories.Contains(type))
		{
			return false;
		//later on the accessory array will probs have fixed locations for Things (ie shooter [0], lens [1], etc)
		//checking if the building is valid is probs easier with a hardcoded enum || something like tower textures
		}
		return false;
	}
	
	public override void DoSelection(bool isBeingSelected)
	{
		base.DoSelection(isBeingSelected);
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
		Vector2 point  = new Vector2(0,-deploySpread);
		float degreesSeparation = (float)((Math.PI * 2)/maxSoldiers);
		point = rallyPoint + point.Rotated(degreesSeparation*soldierArrayPos);
		return point;
	}
	
	public void SpawnSoldier()
	{  
		int soldierAmmount = soldiers.Count;
		if(soldierAmmount < maxSoldiers)
		{
			Soldier soldier = (Soldier)soldierType.Instance();
			((YSort)GetNode("Soldiers")).AddChild(soldier);
			soldier.Position = ((Position2D)GetNode("SpawnPoint")).Position;
			soldier.homePoint = GetPointArroundRally(soldierAmmount);
			soldiers.Add(soldier);
			if((soldierAmmount+1) == maxSoldiers)
			{
				((Timer)GetNode("SoldierSpawnTimer")).Stop();
	
			}
		}
	}
	
	public void OnSoldierSpawnTimerTimeout()
	{
		rallyPoint = GlobalPosition;
		SpawnSoldier();
	
	}
	
	public void OnSoldierKilled(Soldier soldier)
	{
		((Timer)GetNode("SoldierSpawnTimer")).Start(deploySpeed);
	}
}