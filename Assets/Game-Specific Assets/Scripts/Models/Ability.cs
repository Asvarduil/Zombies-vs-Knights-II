using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class Ability : IJsonSavable, INamed, ICloneable
{
    #region Variables / Properties

    public string Name;
    public string Description;
    public string IconPath;
    public Faction Faction;
    public AbilityCommmandTrigger CommandTrigger;
    public AbilityTriggerCondition TriggerCondition;
    public string EffectPath;
    public int ResourceCost;
    //public float LockoutDuration;
    public Lockout Lockout = new Lockout();
    public List<GameEvent> GameEvents;

    public string EntityName { get { return Name; } }

    public bool HasBeenUsed { get; set; }

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
            CommandTrigger = CommandTrigger,
            TriggerCondition = TriggerCondition,
            EffectPath = EffectPath,
            ResourceCost = ResourceCost,
            Lockout = Lockout.Clone() as Lockout,
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
        state["AbilityCommandTrigger"] = new JSONData(CommandTrigger.ToString());
        state["AbilityTriggerCondition"] = new JSONData(TriggerCondition.ToString());
        state["IconPath"] = new JSONData(IconPath);
        state["EffectPath"] = new JSONData(EffectPath);
        state["ResourceCost"] = new JSONData(ResourceCost);
        state["LockoutDuration"] = new JSONData(Lockout.LockoutRate);
        state["GameEvents"] = GameEvents.FoldList();

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Description = state["Description"];
        Faction = state["Faction"].ToEnum<Faction>();
        CommandTrigger = state["AbilityCommandTrigger"].ToEnum<AbilityCommmandTrigger>();
        TriggerCondition = state["AbilityTriggerCondition"].ToEnum<AbilityTriggerCondition>();
        IconPath = state["IconPath"];
        EffectPath = state["EffectPath"];
        ResourceCost = state["ResourceCost"].AsInt;
        Lockout = new Lockout
        {
            LastAttempt = Time.time,
            LockoutRate = state["LockoutDuration"].AsFloat
        };
        GameEvents = state["GameEvents"].AsArray.UnfoldJsonArray<GameEvent>();
    }

    #endregion Methods
}
