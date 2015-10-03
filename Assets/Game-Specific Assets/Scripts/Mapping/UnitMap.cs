using System.Collections.Generic;
using SimpleJSON;

public class UnitMap : IMapper<UnitModel>
{
    public JSONNode MapObjectToJson(UnitModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }

    List<UnitModel> IMapper<UnitModel>.MapJsonToList(JSONNode parsed)
    {
        JSONArray abilities = parsed["Units"].AsArray;
        var result = abilities.UnfoldJsonArray<UnitModel>();
        return result;
    }
}
