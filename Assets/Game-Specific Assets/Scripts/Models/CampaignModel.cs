using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class CampaignModel : IJsonSavable, ICloneable, INamed
{
    public string Name;
    public string Description;
    public string FactionBannerPath;
    public List<CampaignStageModel> Stages;

    public string EntityName
    {
        get { return Name; }
    }

    #region Variables / Properties

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        CampaignModel clone = new CampaignModel
        {
            Name = Name,
            Description = Description,
            FactionBannerPath = FactionBannerPath,
            Stages = Stages.DeepCopyList()
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Description"] = new JSONData(Description);
        state["FactionBannerPath"] = new JSONData(FactionBannerPath);
        state["Stages"] = Stages.FoldList();

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];
        Description = node["Description"];
        FactionBannerPath = node["FactionBannerPath"];
        Stages = node["Stages"].AsArray.UnfoldJsonArray<CampaignStageModel>();
    }

    #endregion Methods
}
