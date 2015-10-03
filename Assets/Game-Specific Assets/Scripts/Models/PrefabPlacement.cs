using System;
using System.Collections.Generic;
using SimpleJSON;

public enum MapObjectType
{
    GameObject,
    Doodad,
    Unit
}

[Serializable]
public class PrefabPlacement : IJsonSavable
{
    #region Fields

    public string Path;
    public string Name;
    public MapObjectType ObjectType;
    public List<PositionRotationPair> Placements;

    #endregion Fields

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Path"] = new JSONData(Path);
        state["Name"] = new JSONData(Name);
        state["ObjectType"] = new JSONData(ObjectType.ToString());
        state["Placements"] = Placements.FoldList();

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Path = node["Path"];
        Name = node["Name"];
        ObjectType = node["ObjectType"].ToEnum<MapObjectType>();
        Placements = node["Placements"].AsArray.UnfoldJsonArray<PositionRotationPair>();
    }

    #endregion Methods
}
