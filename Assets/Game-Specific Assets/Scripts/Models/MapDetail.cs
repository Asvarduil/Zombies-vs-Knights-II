using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class MapDetail : IJsonSavable, ICloneable
{
    #region Fields

    public string Name;
    public string MeshPath;
    public List<string> MaterialPaths;
    public List<string> TexturePaths;
    public List<string> UnitAbilities;
    public List<PrefabPlacement> Placements;

    #endregion Fields

    #region Constructors

    public MapDetail()
    {
        // Stub.
    }

    public MapDetail(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructors

    #region Methods

    public object Clone()
    {
        MapDetail clone = new MapDetail
        {
            Name = Name,
            MeshPath = MeshPath,
            MaterialPaths = MaterialPaths.DeepCopyList(),
            TexturePaths = TexturePaths.DeepCopyList(),
            UnitAbilities = UnitAbilities.DeepCopyList(),
            Placements = Placements.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["MeshPath"] = new JSONData(MeshPath);
        state["MaterialPaths"] = MaterialPaths.FoldPrimitiveList();
        state["TexturePaths"] = TexturePaths.FoldPrimitiveList();
        state["UnitAbilities"] = UnitAbilities.FoldPrimitiveList();
        state["Placements"] = Placements.FoldList();

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        MeshPath = node["MeshPath"];
        MaterialPaths = node["MaterialPaths"].AsArray.UnfoldStringJsonArray();
        TexturePaths = node["TexturePaths"].AsArray.UnfoldStringJsonArray();
        UnitAbilities = node["UnitAbilities"].AsArray.UnfoldStringJsonArray();
        Placements = node["Placements"].AsArray.UnfoldJsonArray<PrefabPlacement>();
    }

    #endregion Methods
}

