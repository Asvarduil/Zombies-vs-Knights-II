using System.Collections.Generic;
using SimpleJSON;

public class CursorMap : IMapper<CursorModel>
{
    public List<CursorModel> MapJsonToList(JSONNode parsed)
    {
        JSONArray cursors = parsed["Cursors"].AsArray;
        var result = cursors.UnfoldJsonArray<CursorModel>();
        return result;
    }

    public JSONNode MapObjectToJson(CursorModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}
