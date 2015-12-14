using System;
using System.Collections.Generic;
using SimpleJSON;

public class AIMap : IMapper<AIModel>
{
    #region Methods

    public JSONNode MapObjectToJson(AIModel sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }

    public List<AIModel> MapJsonToList(JSONNode parsed)
    {
        JSONArray abilities = parsed["AI"].AsArray;
        var result = abilities.UnfoldJsonArray<AIModel>();
        return result;
    }

    #endregion Methods
}
