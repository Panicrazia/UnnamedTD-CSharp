
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class MapBase : Node2D
{
	public const int CELLSize = 16;
	public static readonly Vector2 MAPBounds = new Vector2(63, 37);
    private static readonly Array DIRECTIONS = new Array() { Vector2.Left, Vector2.Right, Vector2.Up, Vector2.Down };
	private static readonly Array DIRECTIONS_REVERSE = new Array(){Vector2.Down, Vector2.Up, Vector2.Right, Vector2.Left };
	
	public Array spawnLocations = new Array(){}	;//could be turned into a dictionary to specify spawning info, probs will be needed later
	public Dictionary<Vector2, object[]> flowFieldPath;
	public Array bottlenecks  = new Array(){} ;//if a slot is 1 then building on a space with that slot as the distance locks the paths
	
	//public __TYPE instance;
	//public __TYPE building;
	
	public PackedScene baddie = GD.Load<PackedScene>("res://Scenes/Baddie.tscn");
	//buildings should be in a dictionary linking their name to their path
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{  
		PopulateSpawnLocationsArray();
		flowFieldPath = GenerateFloodFillPath(new List<Vector2>());
	
	//func _UnhandledInput(event):
	//	if event is InputEventMouseButton:
	//		if event.button_index == BUTTONLeft && event.pressed:
	//			instance = Baddie.Instance();
	//			instance.position = event.position;
	//			instance.velocity = new Vector2(GD.RandRange(25.0, 50.0), 0.0);
	//			AddChild(instance)
	
	
	}
	
	public override void _Process(float delta)
	{  
	
	}

	internal void StartWave(int waveNumber, Wave waveInfo)
	{
		foreach(WaveComponent wave in waveInfo)
		{
			WaveTimer timer = (WaveTimer)GD.Load<PackedScene>("res://Scenes/WaveTimer.tscn").Instance();
			timer.Name = waveNumber + " " + wave.GetBaddieType();
			AddChild(timer, true);
			timer.ticks = wave.GetAmount();
			timer.Connect("timeout", this, "spawn_enemy", new Array(wave.GetBaddieType(), wave.GetHealth(), wave.GetArmor(), wave.GetEffects()));
			timer.Start(wave.GetSeparationTime());
		}
	}

	public void SpawnEnemy(string type, int health, float armor, object[] effects)
	{
		Baddie newEnemy = (Baddie)GD.Load<PackedScene>("res://Scenes/Enemies/" + type + ".tscn").Instance();
		newEnemy.SetSpawnInfo(health, armor, effects);
        //this will not always be the same random in each game, Im not sure if I want to keep that or to make it seeded to the map
        Vector2 spawnloc = (Vector2)spawnLocations[new Random().Next(spawnLocations.Count)];
		((YSort)GetNode("Enemies")).AddChild(newEnemy);
		newEnemy.Position = spawnloc + new Vector2(8,8);
		newEnemy.UpdatePath(flowFieldPath);
	}
	
	public void PopulateSpawnLocationsArray()
	{  
		foreach(Vector2 coordinates in ((TileMap)GetNode("SpawnLocations")).GetUsedCellsById(0))
		{
			spawnLocations.Add(coordinates*CELLSize);
		}
	}
	
	public Wave[] GetWaveInformation()
	{
		return (Wave[])((WaveInformation)GetNode("WaveInformation")).baseWaveInformation.Clone();
	}
	
	public Dictionary<Vector2, object[]> GenerateFloodFillPath(List<Vector2> testLocations)
	{  
		//should only ever be one of these tiles
		Vector2 goal = (Vector2)((TileMap)GetNode("SpawnLocations")).GetUsedCellsById(1)[0];
		TileMap collisionMap = ((TileMap)GetNode("Collisions"));
		TileMap buildingMap = ((TileMap)GetNode("Buildings"));
		Stack < Vector2> stack  = new Stack<Vector2>();
		stack.Push(goal);
		Dictionary<Vector2, object[]> cameFrom  = new Dictionary<Vector2, object[]>();//dictionary of vector2 to an array containing vector2 && distance
		Vector2 current;
		Vector2 offset;

		cameFrom[goal] = new object[] {null, 0};
		//if(testLocations.Empty()):

		((TileMap)GetNode("Pathing")).Clear(); //for visualizing the algorithm, otherwise useless

		while (stack.Count > 0)
		{
			current = stack.Pop();
			offset = goal - current;

			if (((offset.x + offset.y) % 2.0) == 0)
			{
				foreach (int direction in GD.Range(3, -1, -1))
				{
					CheckNeighbors(direction, stack, cameFrom, current, testLocations, collisionMap, buildingMap);
				}
			}
			else
			{
				foreach (int direction in GD.Range(0, 4, 1))
				{
					CheckNeighbors(direction, stack, cameFrom, current, testLocations, collisionMap, buildingMap);
				}
			}
			
		}
		return cameFrom;
	}
	
	public void CheckNeighbors(int direction, Stack<Vector2> stack, Dictionary<Vector2, object[]> cameFrom, Vector2 current, List<Vector2> testLocations, TileMap collisionMap, TileMap buildingMap)
	{
		Vector2 coordinates = current + (Vector2)(DIRECTIONS[direction]);

		if (!(IsOnMapEdge(coordinates) || cameFrom.ContainsKey(coordinates) || testLocations.Contains(coordinates)))
		{
			if (((collisionMap.GetCellv(coordinates) == -1) && (buildingMap.GetCellv(coordinates) == -1)))
			{
				cameFrom[coordinates] = new object[2] { current, 1 + (int)cameFrom[current][1]};

				((TileMap)GetNode("Pathing")).SetCell((int)coordinates.x, (int)coordinates.y, direction); //for visualizing the algorithm, otherwise useless

				stack.Push(coordinates);
			}
		}
	}
	
	public bool SeeIfPathIsViable(List<Vector2> testLocations)
	{
		Array checks = ((TileMap)GetNode("SpawnLocations")).GetUsedCellsById(0);
		Dictionary<Vector2, object[]> feild = GenerateFloodFillPath(testLocations);
		foreach(Vector2 value in checks)
		{
			if((!feild.ContainsKey(value)))
			{
				return false;
			}
		}
		return true;
	}
	
	public bool IsOnMapEdge(Vector2 coords)
	{  
		if(((coords.x < 0) || (coords.x > MAPBounds.x) || (coords.y < 0) || (coords.y > MAPBounds.y)))
		{
			return true;
		}
		return false;
	
	}
	
	public void UpdateEnemyPaths()
	{  
		flowFieldPath = GenerateFloodFillPath(new List<Vector2>());
		foreach(Baddie enemy in ((YSort)GetNode("Enemies")).GetChildren())
		{
			enemy.UpdatePath(flowFieldPath);
		}
	}

 
}