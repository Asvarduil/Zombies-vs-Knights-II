using System;
using System.Collections.Generic;
using SimpleJSON;

public enum UnitAbilityTrigger
{
    None,
    MoveToFriendly,
    MoveToEnemy,
    Defend
}

[Serializable]
public class Ability : IJsonSavable, INamed, ICloneable
{
    #region Variables / Properties

    public string Name;
    public string Description;
    public string IconPath;
    public Faction Faction;
    public UnitAbilityTrigger UnitAbilityTrigger;
    public string EffectPath;
    public int ResourceCost;
    public float LockoutDuration;
    public List<GameEvent> GameEvents;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        Ability clone = new Ability
        {
            Name = Name,
            Description = Description,
            IconPath = IconPath,
            Faction = Faction,
            UnitAbilityTrigger = UnitAbilityTrigger,
            EffectPath = EffectPath,
            ResourceCost = ResourceCost,
            LockoutDuration = LockoutDuration,
            GameEvents = GameEvents.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["Faction"] = new JSONData(Faction.ToString());
        state["UnitAbilityTrigger"] = new JSONData(UnitAbilityTrigger.ToString());
        state["IconPath"] = new JSONData(IconPath);
        state["EffectPath"] = new JSONData(EffectPath);
        state["ResourceCost"] = new JSONData(ResourceCost);
        state["LockoutDuration"] = new JSONData(LockoutDuration);
        state["GameEvents"] = GameEvents.FoldList();

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Description = state["Description"];
        Faction = state["Faction"].ToEnum<Faction>();
        UnitAbilityTrigger = state["UnitAbilityTrigger"].ToEnum<UnitAbilityTrigger>();
        IconPath = state["IconPath"];
        EffectPath = state["EffectPath"];
        ResourceCost = state["ResourceCost"].AsInt;
        LockoutDuration = state["LockoutDuration"].AsFloat;
        GameEvents = state["GameEvents"].AsArray.UnfoldJsonArray<GameEvent>();
    }

    #endregion Methods
}
