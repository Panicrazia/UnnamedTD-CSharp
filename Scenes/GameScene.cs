
using System;
using System.Collections.Generic;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class GameScene : Node2D
{	
	public const int CELL_SIZE = 16;
	public const int WAVETimeBetween = 30;

	public MapBase mapNode;
	public Wave[] waveInformation;
	public int currentWave = 1;

	public bool buildingMode = false;
	public bool buildValid = false;
	public Vector2 buildLocation;
	public Vector2 buildLocationLastCheck;
	public string buildingType;
	public string buildingCategory;
	public int buildingSize  = 0;

	public BuildingBase currentSelection; //should probably make this a node2d and have it determine if things are selectable with an ISelectable interface
										  //(which lets me make things selectable very easily)
	public Dictionary<Vector2, BuildingBase> buildingList = new Dictionary<Vector2, BuildingBase>(); //dictionary mapping vector2 coords to building roots

	public object[] selectionInfo;
	public WeakRef selectionNode  = WeakRef(null);
	
	public Color colorDeny = new Color(1,0,0,.5f);
	public Color colorOkay = new Color(0,1,0,.5f);
	
	//var tower = GD.Load("res://Scenes/BuildingTower.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{  
		//get correct map && store it
		mapNode = (MapBase)GetNode("Map");
	
		//get wave info && generate the actual waves
		waveInformation = mapNode.GetWaveInformation();
		ModifyWaveInformation();
		//bootleg gamestart
		StartWave();
		foreach(TextureButton i in GetTree().GetNodesInGroup("build_buttons"))
		{
			i.Connect("pressed", this, "initiate_build_mode", new Array(){i.Name});
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{

		if (Input.IsActionJustReleased("ui_cancel"))
		{
			if (buildingMode)
			{
				CancelBuildMode();
			}
			else
			{
				if (currentSelection != null)
				{
					Deselect();

				}
			}
		}
		if (Input.IsActionJustReleased("ui_accept"))
		{
			if (buildingMode)
			{
				switch (buildingCategory)
				{
					case "Building":
						TryBuildBuilding();
						break;
					case "Accessory":
						AddAccessory(GetBuildingFromCell(mapNode.GetNode<TileMap>("Collisions").WorldToMap(GetGlobalMousePosition() - mapNode.Position)));
						break;
				}
			}
			else
			{
                SelectBuilding(mapNode.GetNode<TileMap>("Collisions").WorldToMap(GetGlobalMousePosition() - mapNode.Position));
			}
		}
	}
	
	public override void _Process(float delta)
	{  
		if(buildingMode)
		{
			Sprite preview = (Sprite)mapNode.GetNode("BuildingPreview");
			//need custom preview position for accessories for them incase they have issues snapping to things they shouldnt be 
			//allowed to build on in wierd Places (ie on barracks they hug the bottom right)
			switch (buildingCategory)
            {
				case "Building":
					GetBestTilemapTile();
					buildValid = VerifyTilePositionsAsBuildable(buildingSize, buildLocation, (buildLocationLastCheck != buildLocation));
					buildLocationLastCheck = buildLocation;
					preview.GlobalPosition = ((buildLocation - new Vector2(1, 1)) * CELL_SIZE + mapNode.Position);
					break;
				case "Accessory":
					var hoverLocation = ((TileMap)mapNode.GetNode("Collisions")).WorldToMap(GetGlobalMousePosition() - mapNode.Position);
					var snap = ShouldSnap(hoverLocation);
					if ((snap)) //if it should snap then snap to said location
					{
						BuildingBase relevantBuilding = GetBuildingFromCell(hoverLocation);
						buildLocation = relevantBuilding.gridLocation;
						preview.GlobalPosition = (relevantBuilding.Position - new Vector2(CELL_SIZE, CELL_SIZE));
						buildValid = IsAccessoryValid(relevantBuilding);
					}
					else
					{
						preview.GlobalPosition = ((buildLocation - new Vector2(1, 1)) * CELL_SIZE + mapNode.Position);
						GetBestTilemapTile();
						buildValid = false;

					}
					break;
			}
			if((buildValid))
			{
				preview.Modulate = colorOkay;
			}
			else
			{
				preview.Modulate = colorDeny;
			}
		}
		else
		{
			if((selectionInfo.Length > 0 && selectionNode.GetRef() != null )) //might be == null?
			{
				DisplaySelection();
			}
		}
	}
	
	public void DisplaySelection()
	{
		MagicCirclePNG magicalCircle = (MagicCirclePNG)GD.Load<PackedScene>("res://Scenes/MagicCirclePNG.tscn").Instance();
		selectionNode = WeakRef(magicalCircle);
		//selection_info = new Array(){6, Color.deeppink, new Vector2(.2,.2)};
		magicalCircle.SetCircle((int)selectionInfo.GetValue(0));
		magicalCircle.SetColor((Color)selectionInfo.GetValue(1));
		magicalCircle.Scale = ((Vector2)selectionInfo.GetValue(2));
		GetNode("UI").AddChild(magicalCircle);
		magicalCircle.Activate();
		magicalCircle.Position = currentSelection.Position;
	}
	
	public void SelectBuilding(Vector2 cell)
	{  
		if(currentSelection != null)
		{
			Deselect();
		}
		currentSelection = GetBuildingFromCell(cell);
		if(currentSelection != null)
		{
			currentSelection.DoSelection(true);
			selectionInfo = currentSelection.GetSelectionInfo();
			//have tower get an outline
			//_unit_overlay.Draw(_walkableCells)
	
		}
	}
	
	public void Deselect()
	{  
		currentSelection.DoSelection(false);
		currentSelection = null;
		selectionInfo = new object[0];
		MagicCirclePNG circle = (MagicCirclePNG)selectionNode.GetRef();
		if (circle != null)
		{
			circle.Hide();
			circle.QueueFree();
			//selection_node == null
		}
	}
	
	public BuildingBase GetBuildingFromCell(Vector2 cell)
	{   //returns null if no building is present
		TileMap buildingsNode = mapNode.GetNode<TileMap>("Buildings");
		int cellValue = buildingsNode.GetCellv(cell);
		if (cellValue != -1)
		{
			while((cellValue != 21)) //this is the value it will be if it is the bottom right corner
			{
				if((cellValue % 3 != 0)) //if !on bottom shift downwards
				{
					cell += new Vector2(0,1);
				}
				if((cellValue % 7 != 0)) //if !on rightside shift right
				{
					cell += new Vector2(1,0);
				}
				cellValue = buildingsNode.GetCellv(cell);
			}
		}
		if(!buildingList.ContainsKey(cell))
		{
			return null;
		}
		return buildingList[cell];
	}

	public void CancelBuildMode()
	{  
		buildingMode = false;
		buildValid = false;
		mapNode.GetNode<Sprite>("BuildingPreview").Hide();
		//$UI/BuildingPreview.Hide()
	
	}
	
	public void InitiateBuildMode(String type)
	{  
		if(buildingMode)
		{
			CancelBuildMode();
		}
		buildingMode = true;
		buildingType = type;
		switch (type)
		{
			case "Tower":
				buildingCategory = "Building";
				buildingSize = 2;
				break;
			case "Barracks":
				buildingCategory = "Building";
				buildingSize = 3;
				break;
			case "Orb":
				buildingCategory = "Accessory";
				buildingSize = 2;
				break;

		}
		SetupPreview(buildingType, buildingCategory);
	}
	
	public void SetupPreview(String type, String category)
	{  
		Sprite preview = (Sprite)mapNode.GetNode("BuildingPreview");
		preview.Texture = (Texture)GD.Load("res://Assets/"+category+"/"+category+type+".png");
		preview.Show();
	
	}
	
	public void TryBuildBuilding()
	{  
		if(buildValid)
		{
			BuildingBase newBuilding = (BuildingBase)GD.Load<PackedScene>("res://Scenes/" + buildingCategory + "/" + buildingCategory + buildingType + ".tscn").Instance();
			
			newBuilding.Position = buildLocation*CELL_SIZE;
			if(buildingSize%2 == 1)
			{
				newBuilding.Position += new Vector2(CELL_SIZE/2, CELL_SIZE / 2);
			}
			mapNode.GetNode("Towers").AddChild(newBuilding, true);
			Vector2 gridLocation = MakeUnbuildable(buildingSize, buildLocation);
			buildingList[gridLocation] = newBuilding;
			newBuilding.gridLocation = gridLocation;
			CancelBuildMode();
	
		}
	}
	
	public bool VerifyTilePositionsAsBuildable(int size, Vector2 pos, bool checkCollisions)
	{  
		if((checkCollisions))
		{
			TileMap collisions = mapNode.GetNode<TileMap>("Collisions");
			List<Vector2> positions = new List<Vector2>();
			for (int xVal=0; xVal < size; xVal++)
			{
				for(int yVal= 0; yVal < size; yVal++)
				{
					positions.Add(pos + new Vector2((float)(xVal - Math.Floor(size / 2.0)), (float)(yVal - Math.Floor(size / 2.0))));
				}
			}
			foreach(Vector2 value in positions)
			{
				int check = collisions.GetCellv(value);
				if((check == 0) || (check == 1) || (mapNode.GetNode<TileMap>("Buildings").GetCellv(value) != -1))
				{
					return false;
				}
			}
			if((mapNode.SeeIfPathIsViable(positions)))
			{
				return true;
			}
			return false;
		}
		return buildValid;
	
	//causes the collision && building maps to reflect the new building, returns the lowest right tile it built on
	}
	
	public Vector2 MakeUnbuildable(int size, Vector2 pos)
	{
		//var collisions = mapNode.GetNode("Collisions");
		TileMap buildingMap = mapNode.GetNode<TileMap>("Buildings");
		Vector2 mapPos = Vector2.Zero;
		int numberForBuildingMap = 1;
		for (int xVal = 0; xVal < size; xVal++)
		{
			for (int yVal = 0; yVal < size; yVal++)
			{
				mapPos = (pos + new Vector2((float)(xVal - Math.Floor(size/2.0)), (float)(yVal - Math.Floor(size / 2.0))));
				//collisions.SetCellv(mapPos, 0)
				if(size == 1)
				{
					numberForBuildingMap = 13;
				}
				else
				{
					if(yVal == 0)
					{
						numberForBuildingMap *= 2; //top
					}
					else if (yVal == (size-1))
					{
						numberForBuildingMap *= 3; //bottom
					}
					if(xVal == 0)
					{
						numberForBuildingMap *= 5; //left
					}
					else if (xVal == (size-1))
					{
						numberForBuildingMap *= 7; //right
					}
				}
				buildingMap.SetCellv(mapPos, numberForBuildingMap);
				numberForBuildingMap = 1;
		//collisions.UpdateDirtyQuadrants()
			}
		}
		buildingMap.UpdateDirtyQuadrants();
		mapNode.UpdateEnemyPaths();
		return mapPos;
	
	//finds the best tile to use for even sized buildings
	}
	
	public void GetBestTilemapTile()
	{  
		//might need to be global position for map?
		Vector2 pos  = (GetGlobalMousePosition() - mapNode.Position);
		int xVal  = 0;
		int yVal  = 0;
		if(buildingSize % 2 == 0)
		{
			xVal = (int)pos.x;
			yVal = (int)pos.y;
			xVal %= CELL_SIZE;
			yVal %= CELL_SIZE;
			//The below two if statements were weird in the GDScript version but if things fuckup seemingly at this algorithm then it might be here?
			if(xVal < CELL_SIZE / 2)
			{
				xVal -= CELL_SIZE;
			}
			if(yVal < CELL_SIZE / 2)
			{
				yVal -= CELL_SIZE;
			}
		}
		buildLocation = ((TileMap)mapNode.GetNode("Collisions")).WorldToMap((new Vector2(xVal, yVal) + pos));
	
	}
	
	public bool ShouldSnap(Vector2 cell)
	{  
		var cellValue = ((TileMap)mapNode.GetNode("Buildings")).GetCellv(cell);
		if(cellValue != -1)
		{
			return true;
		}
		return false;
	//		while (cellValue != 21): #this is the value it will be if it is the bottom right corner
	//			if(cellValue % 3 != 0): #if !on bottom shift downwards
	//				cell += new Vector2(0,1);
	//			if(cellValue % 7 != 0): #if !on rightside shift right
	//				cell += new Vector2(1,0);
	//			cellValue = mapNode.GetNode("Buildings").GetCellv(cell);
	//		return cell
	//	return new Vector2(0,0)
	
	
	// are these two accessory methods possibly useless? yes, but I have them just incase
	}
	
	public bool IsAccessoryValid(BuildingBase relevantBuilding)
	{  
		return relevantBuilding.CheckAccessoryValid(buildingType);
	
	}
	
	public void AddAccessory(BuildingBase relevantBuilding)
	{  
		if(buildValid)
		{
			relevantBuilding.AddAccessory(buildingType);
			CancelBuildMode();
	
		}
	}
	
	public void ModifyWaveInformation()
	{  
		
	
	}
	
	public void StartWave()
	{  
		if(currentWave <= waveInformation.Length)
		{
			if(currentWave <= 1)
			{
				GetNode<Timer>("WaveTimerBetween").Start(WAVETimeBetween);
            }
			mapNode.StartWave(currentWave, waveInformation[currentWave]);
			currentWave += 1;
		}
	}
}