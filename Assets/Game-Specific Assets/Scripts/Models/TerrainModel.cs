using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class TerrainModel : IJsonSavable, ICloneable
{
    #region Fields

    public string MeshPath;
    public List<MaterialModel> Materials;

    #endregion Fields

    #region Constructor

    public TerrainModel()
    {

    }

    public TerrainModel(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructor

    #region Methods

    public object Clone()
    {
        TerrainModel clone = new TerrainModel
        {
            MeshPath = MeshPath,
            Materials = Materials.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        throw new InvalidOperationException("TerrainModels are read-only.");
    }

    public void ImportState(JSONClass node)
    {
        MeshPath = node["MeshPath"];
        Materials = node["Materials"].AsArray.UnfoldJsonArray<MaterialModel>();
    }

    #endregion Methods
}
