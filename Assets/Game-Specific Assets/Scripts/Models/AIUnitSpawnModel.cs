using System;
using System.Collections.Generic;
using SimpleJSON;

public enum AIUnitCompEvaluationPool
{
    AI,
    Player,
    All
}

[Serializable]
public class AIUnitSpawnModel : IJsonSavable, ICloneable
{
    #region Fields

    public string Name;
    public AIUnitCompEvaluationPool EvaluationPool;
    public List<string> CompositionCondition;
    public List<string> TargetComposition;

    #endregion Fields

    #region Methods

    public object Clone()
    {
        AIUnitSpawnModel clone = new AIUnitSpawnModel
        {
            Name = Name,
            EvaluationPool = EvaluationPool,
            CompositionCondition = CompositionCondition.DeepCopyList(),
            TargetComposition = TargetComposition.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        // Does not save state.
        throw new NotImplementedException();
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        EvaluationPool = node["EvaluationPool"].ToEnum<AIUnitCompEvaluationPool>();
        CompositionCondition = node["CompositionCondition"].AsArray.UnfoldStringJsonArray();
        TargetComposition = node["TargetComposition"].AsArray.UnfoldStringJsonArray();
    }

    #endregion Methods
}
