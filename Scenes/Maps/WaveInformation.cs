
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public class WaveInformation : Node
{
	 
	public Dictionary baseWaveInformation = new Dictionary(){
		1:new Array(){new Array(){"OrcTroop", 10, 1, 15, 0,new Array(){}}},
		2:new Array(){new Array(){"OrcTroop", 30, .3, 15, 0,new Array(){}}}
		};
	//format currently is: new Array(){new Array(){"type1", amount, separationTime, baseHealth, baseArmor, new Array(){list of effects they would have}}, new Array(){"type2", amount, separationTime, baseHealth, baseArmor, new Array(){list of effects they would have}}, etc}
	
	// Called when the node enters the scene tree for the first time.
	public void _Ready()
	{  
		pass ;// Replace with function body.
	
	
	
	
	
	}
	
	
	
}