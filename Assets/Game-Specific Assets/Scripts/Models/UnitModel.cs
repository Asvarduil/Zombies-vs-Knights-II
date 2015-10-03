using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class UnitModel : IJsonSavable, INamed, ICloneable
{
    #region Fields

    public string Name;
    public bool IsKeyUnit;
    public Faction Faction;
    public List<ModifiableStat> Stats;
    public List<string> AbilityNames;
    public string MeshPath;
    public string DeathObjectPath;

    public string EntityName { get { return Name; } }

    #endregion Fields

    #region Methods

    public object Clone()
    {
        UnitModel clone = new UnitModel
        {
            Name = Name,
            IsKeyUnit = IsKeyUnit,
            Faction = Faction,
            Stats = Stats.DeepCopyList(),
            MeshPath = MeshPath,
            DeathObjectPath = DeathObjectPath
        };

        clone.AbilityNames = new List<string>();
        for(int i = 0; i < AbilityNames.Count; i++)
        {
            string current = AbilityNames[i];
            clone.AbilityNames.Add(current);
        }

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["IsKeyUnit"] = new JSONData(IsKeyUnit);
        state["Faction"] = new JSONData(Faction.ToString());
        state["Stats"] = Stats.FoldList();
        state["AbilityNames"] = AbilityNames.FoldPrimitiveList();
        state["MeshPath"] = new JSONData(MeshPath);
        state["DeathObjectPath"] = new JSONData(DeathObjectPath);

        return state;
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        IsKeyUnit = state["IsKeyUnit"].AsBool;
        Faction = state["Faction"].ToEnum<Faction>();
        Stats = state["Stats"].AsArray.UnfoldJsonArray<ModifiableStat>();
        AbilityNames = state["AbilityNames"].AsArray.UnfoldStringJsonArray();
        MeshPath = state["MeshPath"];
        DeathObjectPath = state["DeathObjectPath"];
    }

    #endregion Methods
}
