using System.Collections.Generic;
using SimpleJSON;

public interface IMapper<T>
    where T : IJsonSavable
{
    List<T> MapJsonToList(JSONNode parsed);
    JSONNode MapObjectToJson(T sourceObject);
}
