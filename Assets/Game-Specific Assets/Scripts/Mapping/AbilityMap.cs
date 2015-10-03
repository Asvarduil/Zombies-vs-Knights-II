using System.Collections.Generic;
using SimpleJSON;

public class AbilityMap : IMapper<Ability>
{
    public JSONNode MapObjectToJson(Ability sourceObject)
    {
        JSONNode result = sourceObject.ExportState();
        return result;
    }

    public List<Ability> MapJsonToList(JSONNode parsed)
    {
        JSONArray abilities = parsed["Abilities"].AsArray;
        var result = abilities.UnfoldJsonArray<Ability>();
        return result;
    }
}

