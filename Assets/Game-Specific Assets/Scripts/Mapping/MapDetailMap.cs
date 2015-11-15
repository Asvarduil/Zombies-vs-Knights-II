using System.Collections.Generic;
using SimpleJSON;

public class MapDetailMap : IMapper<MapModel>
{
    public List<MapModel> MapJsonToList(JSONNode parsed)
    {
        JSONArray mapDetails = parsed["MapDetail"].AsArray;
        var result = mapDetails.UnfoldJsonArray<MapModel>();
        return result;
    }

    public JSONNode MapObjectToJson(MapModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}

