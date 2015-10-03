using System;
using System.Collections.Generic;
using SimpleJSON;

public class BuffMap : IMapper<Buff>
{
    public List<Buff> MapJsonToList(JSONNode mapped)
    {
        JSONArray abilities = mapped["Buffs"].AsArray;
        var result = abilities.UnfoldJsonArray<Buff>();
        return result;
    }

    public JSONNode MapObjectToJson(Buff sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }
}
