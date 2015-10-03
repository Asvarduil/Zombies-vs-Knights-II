﻿using System;
using SimpleJSON;

public enum TargetType
{
    Self,
    Cursor,
    FriendlySpawnSphere
}

[Serializable]
public class Ability : IJsonSavable, INamed, ICloneable
{
    #region Variables / Properties

    public string Name;
    public string Description;
    public TargetType TargetType;
    public string IconPath;
    public string EffectPath;
    public int ResourceCost;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        Ability clone = new Ability
        {
            Name = Name,
            Description = Description,
            TargetType = TargetType,
            IconPath = IconPath,
            EffectPath = EffectPath,
            ResourceCost = ResourceCost
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["TargetType"] = new JSONData(TargetType.ToString());
        state["IconPath"] = new JSONData(IconPath);
        state["EffectPath"] = new JSONData(EffectPath);
        state["ResourceCost"] = new JSONData(ResourceCost);

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Description = state["Description"];
        TargetType = state["TargetType"].ToEnum<TargetType>();
        IconPath = state["IconPath"];
        EffectPath = state["EffectPath"];
        ResourceCost = state["ResourceCost"].AsInt;
    }

    #endregion Methods
}
