using System;
using SimpleJSON;

[Serializable]
public class ModifiableStat : IEntityStat, IJsonSavable, INamed, ICloneable
{
	#region Variables / Properties

	public string Name;
	public int Value;
    public int ValueCap;

	public int FixedModifier = 0;
	public float ScalingModifier = 1.0f;

    public string EntityName { get { return Name; } }

    public int Current
    {
        get { return ModifiedValue; }
        set { Value = value; }
    }

    public int Maximum
    {
        get { return ValueCap; }
        set { ValueCap = value; }
    }

	public int ModifiedValue
	{
		get 
        { 
            int modified = ((int)(Value * ScalingModifier)) + FixedModifier;
            return modified > ValueCap
                ? ValueCap
                : modified;
        }
	}

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        ModifiableStat clone = new ModifiableStat
        {
            Name = Name,
            Value = Value,
            ValueCap = ValueCap,
            FixedModifier = FixedModifier,
            ScalingModifier = ScalingModifier
        };

        return clone;
    }

    public void RaiseMax(int amount)
    {
        ValueCap += amount;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Value"] = new JSONData(Value);
        state["ValueCap"] = new JSONData(ValueCap);
        state["FixedModifier"] = new JSONData(FixedModifier);
        state["ScalingModifier"] = new JSONData(ScalingModifier);

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Value = state["Value"].AsInt;
        ValueCap = state["ValueCap"].AsInt;
        FixedModifier = state["FixedModifier"].AsInt;
        ScalingModifier = state["ScalingModifier"].AsFloat;
    }

    #endregion Methods
}
