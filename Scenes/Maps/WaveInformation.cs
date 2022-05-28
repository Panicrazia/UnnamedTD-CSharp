
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;
using System.Collections.Generic;
using System.Collections;

public class WaveInformation : Node
{
	 
	public Wave[] baseWaveInformation = new Wave[]{
		new Wave(new WaveComponent("OrcTroop", 10, 1f, 1.4f, 0, new object[]{})),
        new Wave(new WaveComponent("OrcTroop", 30, .07f, 2, 0, new object[]{}))
        };
    //format currently is:

    //new Array(){
    //new Array(){"type1", amount, separationTime, baseHealthMultiplier, baseArmorMultiplier, new Array(){list of effects they would have}},
    //new Array(){"type2", amount, separationTime, baseHealthMultiplier, baseArmorMultiplier, new Array(){list of effects they would have}}, etc
    //}

    /*
	 * should make it so its an array of waves, and each wave contains the information needed
	 */

}

public class Wave : IEnumerable//<WaveComponent>
{
    private List<WaveComponent> _waveComponent = new List<WaveComponent>();

    public Wave(WaveComponent waveComponent)//constructor needs to take variable arguments, all wavecomponents
    {
        _waveComponent.Add(waveComponent);
    }

    public WaveEnum GetEnumerator()
    {
        return new WaveEnum(_waveComponent);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator)GetEnumerator();
    }
    /*
    IEnumerator<WaveComponent> IEnumerable<WaveComponent>.GetEnumerator()
    {
        return ((IEnumerable<WaveComponent>)_waveComponent).GetEnumerator();
    }*/
}

public class WaveEnum : IEnumerator
{
    public List<WaveComponent> _waveComponent;

    // Enumerators are positioned before the first element
    // until the first MoveNext() call.
    int position = -1;

    public WaveEnum(List<WaveComponent> list)
    {
        _waveComponent = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _waveComponent.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public WaveComponent Current
    {
        get
        {
            try
            {
                return _waveComponent[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}


public class WaveComponent
{
    public string BaddieType { get; }
    public int Amount { get; }
    public float SeparationTime { get; }
    public float BaseHealthMultiplier { get; }
    public float BaseArmorMultiplier { get; }
    public object[] Effects { get; }

    public WaveComponent(string baddieType, int amount, float separationTime, float baseHealthMultiplier, float baseArmorMultiplier, object[] effects)
    {
        this.BaddieType = baddieType;
        this.Amount = amount;
        this.SeparationTime = separationTime;
        this.BaseHealthMultiplier = baseHealthMultiplier;
        this.BaseArmorMultiplier = baseArmorMultiplier;
        this.Effects = effects;
    }
}