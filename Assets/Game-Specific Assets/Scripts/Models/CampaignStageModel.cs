using System;
using SimpleJSON;

[Serializable]
public class CampaignStageModel : IJsonSavable, ICloneable, INamed
{
    #region Variables / Properties

    public string Name;
    public string Description;
    public Faction PlayerFaction;
    public bool UsesCustomMap;
    public string MapName;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Method

    public object Clone()
    {
        CampaignStageModel clone = new CampaignStageModel
        {
            Name = Name,
            Description = Description,
            PlayerFaction = PlayerFaction,
            UsesCustomMap = UsesCustomMap,
            MapName = MapName
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["PlayerFaction"] = new JSONData(PlayerFaction.ToString());
        state["UsesCustomMap"] = new JSONData(UsesCustomMap);
        state["MapName"] = new JSONData(MapName);

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        Description = node["Description"];
        PlayerFaction = node["PlayerFaction"].ToEnum<Faction>();
        UsesCustomMap = node["UsesCustomMap"].AsBool;
        MapName = node["MapName"];
    }

    #endregion Methods
}
