
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using System.Collections.Generic;
using System.Collections;

public class WaveInformation : Node
{
	 
	public Wave[] baseWaveInformation = new Wave[]{
		new Wave(new WaveComponent("OrcTroop", 10, 1, 15, 0, new Array(){})),
        new Wave(new WaveComponent("OrcTroop", 10, 1, 15, 0, new Array(){}))
        };
	//format currently is:
	//new Array(){
	//new Array(){"type1", amount, separationTime, baseHealth, baseArmor, new Array(){list of effects they would have}},
	//new Array(){"type2", amount, separationTime, baseHealth, baseArmor, new Array(){list of effects they would have}}, etc
	//}

	/*
	 * should make it so its an array of waves, and each wave contains the information needed
	 */

}

public class Wave : IEnumerable<WaveComponent>
{
    private WaveComponent waveComponent;

    public Wave(WaveComponent waveComponent)//constructor needs to take variable arguments, all wavecomponents
    {
        this.waveComponent = waveComponent;
    }

    public IEnumerator<WaveComponent> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

public class WaveComponent
{
    private string v1;
    private int v2;
    private int v3;
    private int v4;
    private int v5;
    private Array array;

    public WaveComponent(string v1, int v2, int v3, int v4, int v5, Array array)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
        this.v5 = v5;
        this.array = array;
    }

    internal string GetBaddieType()
    {
        throw new NotImplementedException();
    }

    internal int GetAmount()
    {
        throw new NotImplementedException();
    }

    internal float GetSeparationTime()
    {
        throw new NotImplementedException();
    }

    internal int GetHealth()
    {
        throw new NotImplementedException();
    }

    internal int GetArmor()
    {
        throw new NotImplementedException();
    }

    internal int GetEffects()
    {
        throw new NotImplementedException();
    }
}