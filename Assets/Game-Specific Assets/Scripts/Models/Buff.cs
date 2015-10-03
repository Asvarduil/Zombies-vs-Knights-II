using System;
using SimpleJSON;

[Serializable]
public class Buff : IJsonSavable, INamed, ICloneable
{
    #region Constants

    private const float CheckEffectTickEpsilon = 0.001f;

    #endregion Constants

    #region Variables / Properties

    public string Name;
    public string Description;
    public string AffectedStat;
    public int EffectValue;
    public float EffectRate;
    public float EffectDuration;
    public float EffectStartTime;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        Buff clone = new Buff
        {
            Name = Name,
            Description = Description,
            AffectedStat = AffectedStat,
            EffectValue = EffectValue,
            EffectRate = EffectRate,
            EffectDuration = EffectDuration
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["AffectedStat"] = new JSONData(AffectedStat);
        state["EffectValue"] = new JSONData(EffectValue);
        state["EffectRate"] = new JSONData(EffectRate);
        state["EffectDuration"] = new JSONData(EffectDuration);

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Description = state["Description"];
        AffectedStat = state["AffectedStat"];
        EffectValue = state["EffectValue"].AsInt;
        EffectRate = state["EffectRate"].AsFloat;
        EffectDuration = state["EffectDuration"].AsFloat;
    }

    public bool HasExpired(float currentTime)
    {
        return currentTime > EffectStartTime + EffectDuration;
    }

    public bool TickHasOccurred(float currentTime)
    {
        if (EffectRate == 0.0f)
            return false;

        return Math.Abs((currentTime - EffectStartTime) % EffectRate - 0.0f) < CheckEffectTickEpsilon;
    }

    #endregion Methods
}
