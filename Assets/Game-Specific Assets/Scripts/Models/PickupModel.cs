using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class PickupModel : INamed, ICloneable, IJsonSavable
{
    #region Variables / Properties

    public string Name;
    public int ResourceYield;
    public string MeshPath;
    public List<GameEvent> GameEvents;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        PickupModel clone = new PickupModel
        {
            Name = Name,
            ResourceYield = ResourceYield,
            MeshPath = MeshPath,
            GameEvents = GameEvents.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass result = new JSONClass();

        // TODO: We will never save a Pickup, for any reason.

        return result;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        ResourceYield = node["ResourceYield"].AsInt;
        MeshPath = node["MeshPath"];
        GameEvents = node["GameEvents"].AsArray.UnfoldJsonArray<GameEvent>();
    }

    #endregion Methods
}
