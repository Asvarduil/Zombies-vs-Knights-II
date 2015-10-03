using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class MapDetail : IJsonSavable
{
    #region Fields

    public string Name;
    public List<PrefabPlacement> Placements;

    #endregion Fields

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Placements"] = Placements.FoldList();

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        Placements = node["Placements"].AsArray.UnfoldJsonArray<PrefabPlacement>();
    }

    #endregion Methods

}

