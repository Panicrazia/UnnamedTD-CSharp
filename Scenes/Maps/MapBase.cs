
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class MapBase : Node2D
{
	public const int CELLSize = 16;
	public static readonly Vector2 MAPBounds = new Vector2(63, 37);
	public static readonly Array DIRECTIONS = new Array(){Vector2.LEFT, Vector2.RIGHT, Vector2.UP, Vector2.DOWN};
	public static readonly Array DIRECTIONSReverse = new Array(){Vector2.DOWN, Vector2.UP, Vector2.RIGHT, Vector2.LEFT};
	
	public Array spawnLocations = new Array(){}	;//could be turned into a dictionary to specify spawning info, probs will be needed later
	public __TYPE flowFieldPath;
	public Array bottlenecks  = new Array(){} ;//if a slot is 1 then building on a space with that slot as the distance locks the paths
	
	public __TYPE instance;
	public __TYPE building;
	
	public __TYPE baddie = GD.Load("res://Scenes/baddie.tscn");
	//buildings should be in a dictionary linking their name to their path
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		PopulateSpawnLocationsArray();
		flowFieldPath = GenerateFloodFillPath(new Array(){});
		pass ;// Replace with function body.
	
	//func _UnhandledInput(event):
	//	if event is InputEventMouseButton:
	//		if event.button_index == BUTTONLeft && event.pressed:
	//			instance = baddie.Instance();
	//			instance.position = event.position;
	//			instance.velocity = new Vector2(GD.RandRange(25.0, 50.0), 0.0);
	//			AddChild(instance)
	
	
	}
	
	public void _Process(__TYPE delta)
	{  
	
	}
	
	public void StartWave(__TYPE waveNumber, __TYPE waveInfo)
	{  
		//[new Array(){"type", amount, separationTime, health, armor, new Array(){list of effects they would have}}]
		foreach(var enemyInformation in waveInfo)
		{
			var timer = GD.Load("res://Scenes/WaveTimer.tscn").Instance();
			timer.name = (waveNumber as String) + enemyInformation[0];
			AddChild(timer, true);
			timer.countdown_var = enemyInformation[1]
			timer.Connect("timeout", this, "spawn_enemy", [enemyInformation[0],enemyInformation[3],enemyInformation[4],enemyInformation[5]]);
			timer.Start(enemyInformation[2]);
	
		}
	}
	
	public void SpawnEnemy(__TYPE type, __TYPE health, __TYPE armor, __TYPE effects)
	{  
		var newEnemy = GD.Load("res://Scenes/Enemies/" + type + ".tscn").Instance();
		newEnemy.SetSpawnInfo(health, armor, effects)
		//this will always be the same random in each game, Im !sure if I want to keep that || !var spawnloc = spawnLocations[GD.Randi() % spawnLocations.Size()];
		$Enemies.AddChild(newEnemy)
		newEnemy.position = spawnloc + new Vector2(8,8);
		newEnemy.UpdatePath(flowFieldPath)
	
	}
	
	public void PopulateSpawnLocationsArray()
	{  
		foreach(var coordinates in $SpawnLocations.GetUsedCellsById(0))
		{
			spawnLocations.Append(coordinates*CELL_SIZE);
		}
	
	}
	
	public Dictionary GetWaveInformation()
	{  
		return $WaveInformation.base_wave_information.Duplicate(true)
	
	}
	
	public Dictionary GenerateFloodFillPath(Array testLocations)
	{  
		//should only ever be one of these tiles
		var start = $SpawnLocations.GetUsedCellsById(1)[0]
		Array stack  = new Array(){start};
		Dictionary cameFrom  = new Dictionary(){} ;//dictionary of vector2 to an array containing vector2 && distance
		var current;
		var offset;
		
		cameFrom[start] = new Array(){null, 0};
		//if(testLocations.Empty()):
		$Pathing.Clear()
		
		//apparently godot has issues with doing this recursively for whatever reason, if I knew more about threads it probs could be done that way though
		while(!stack.Empty())
		{
			current = stack.PopFront();
			offset = start - current;
			
			//I dont think a distance check is needed but heres some code incase it somehow becomes relevant
			//Vector2 difference = (current - cell).Abs();
			//var distance := (int)(difference.x + difference.y)
			//if distance > maxDistance:
			//	continue
			
			if(((offset.x as int + offset.y as int)%2))
			{
				foreach(var direction in GD.Range(3,-1,-1))
				{
					CheckNeighbors(direction, stack, cameFrom, current, testLocations);
				}
			}
			else
			{
				foreach(var direction in GD.Range(0,4,1))
				{
					CheckNeighbors(direction, stack, cameFrom, current, testLocations);
				}
			}
		}
		return cameFrom;
	
	}
	
	public void CheckNeighbors(int direction, Array stack, Dictionary cameFrom, Vector2 current, Array testLocations)
	{  
		Vector2 coordinates = current + (DIRECTIONS[direction]);
		if(coordinates in cameFrom)
		{
			return;
		}
		if(testLocations.Has(coordinates))
		{
			return;
		}
		if((($Collisions.GetCellv(coordinates) == -1) && ($Buildings.GetCellv(coordinates) == -1)))
		{
			cameFrom[coordinates] = [current,(1 + cameFrom[current][1])]
			
			$Pathing.SetCell(coordinates.x, coordinates.y, direction)
			
			if(!is_on_map_edge(coordinates))
			{
				stack.Append(coordinates);
	
			}
		}
	}
	
	public bool SeeIfPathIsViable(Array testLocations)
	{  
		var checks = $SpawnLocations.GetUsedCellsById(0)
		var feild = GenerateFloodFillPath(testLocations);
		foreach(var value in checks)
		{
			if((!feild.Has(value)))
			{
				return false;
			}
		}
		return true;
	
	}
	
	public __TYPE IsOnMapEdge(__TYPE coords)
	{  
		if(((coords.x < 0) || (coords.x > MAPBounds.x) || (coords.y < 0) || (coords.y > MAPBounds.y)))
		{
			return true;
		}
		return false;
	
	}
	
	public void UpdateEnemyPaths()
	{  
		flowFieldPath = GenerateFloodFillPath(new Array(){});
		foreach(var enemy in $Enemies.GetChildren())
		{
			enemy.UpdatePath(flowFieldPath);
	
	
		}
	}
	
	
	
}