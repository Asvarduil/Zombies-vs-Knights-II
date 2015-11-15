using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class MapModel : IJsonSavable, ICloneable
{
    #region Fields

    public string Name;
    public TerrainModel Terrain;
    public List<string> UnitAbilities;
    public List<PrefabPlacement> Placements;

    #endregion Fields

    #region Constructors

    public MapModel()
    {
        // Stub.
    }

    public MapModel(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructors

    #region Methods

    public object Clone()
    {
        MapModel clone = new MapModel
        {
            Name = Name,
            Terrain = Terrain.DeepCopy(),
            UnitAbilities = UnitAbilities.DeepCopyList(),
            Placements = Placements.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        throw new InvalidOperationException("MapModels are read-only.");
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        Terrain = new TerrainModel(node["Terrain"].AsObject);
        UnitAbilities = node["UnitAbilities"].AsArray.UnfoldStringJsonArray();
        Placements = node["Placements"].AsArray.UnfoldJsonArray<PrefabPlacement>();
    }

    #endregion Methods
}

