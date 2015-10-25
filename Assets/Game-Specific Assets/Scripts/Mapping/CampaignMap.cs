using System.Collections.Generic;
using SimpleJSON;

public class CampaignMap : IMapper<CampaignModel>
{
    public List<CampaignModel> MapJsonToList(JSONNode parsed)
    {
        JSONArray campaigns = parsed["Campaigns"].AsArray;
        var result = campaigns.UnfoldJsonArray<CampaignModel>();
        return result;
    }

    public JSONNode MapObjectToJson(CampaignModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}
