using System.Collections.Generic;
using SimpleJSON;

public class PickupMap : IMapper<PickupModel>
{
    public List<PickupModel> MapJsonToList(JSONNode parsed)
    {
        JSONArray abilities = parsed["Pickups"].AsArray;
        var result = abilities.UnfoldJsonArray<PickupModel>();
        return result;
    }

    public JSONNode MapObjectToJson(PickupModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}
