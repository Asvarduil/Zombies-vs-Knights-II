using System.Collections.Generic;
using SimpleJSON;

public class MapDetailMap : IMapper<MapDetail>
{
    public List<MapDetail> MapJsonToList(JSONNode parsed)
    {
        JSONArray mapDetails = parsed["MapDetail"].AsArray;
        var result = mapDetails.UnfoldJsonArray<MapDetail>();
        return result;
    }

    public JSONNode MapObjectToJson(MapDetail sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}

