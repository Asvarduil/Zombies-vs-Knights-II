using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class AIModel : IJsonSavable, ICloneable, INamed
{
    #region Fields

    public string Name;
    public List<AIUnitSpawnModel> SpawnConditions;
    public List<AIUnitTargetingModel> TargetingConditions;

    public string EntityName
    {
        get { return Name; }
    }

    #endregion Fields

    #region Methods

    public object Clone()
    {
        AIModel clone = new AIModel
        {
            Name = Name,
            SpawnConditions = SpawnConditions.DeepCopyList(),
            TargetingConditions = TargetingConditions.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        // TODO: Load-only model.

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        SpawnConditions = node["SpawnConditions"].AsArray.UnfoldJsonArray<AIUnitSpawnModel>();
        TargetingConditions = node["TargetingConditions"].AsArray.UnfoldJsonArray<AIUnitTargetingModel>();
    }

    #endregion Methods
}
