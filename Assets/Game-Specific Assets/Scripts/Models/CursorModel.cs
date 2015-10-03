using System;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class CursorModel : INamed, IJsonSavable, ICloneable
{
    #region Variables / Properties

    public string Name;
    public Texture2D Texture;
    public Vector2 Point;

    private string _texturePath;

    public string EntityName { get { return Name; } }

    #endregion Variables / Properties

    #region Methods

    public object Clone()
    {
        CursorModel clone = new CursorModel
        {
            Name = Name,
            Texture = Texture,
            Point = Point
        };

        return clone;
    }

    public void ImportState(JSONClass node)
    {
        Name = node["Name"];

        _texturePath = node["Texture"];
        Texture = Resources.Load<Texture2D>(_texturePath);

        Point = node["Point"].ImportVector2();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Texture"] = new JSONData(_texturePath);
        state["Point"] = Point.ExportAsJson();

        return state;
    }

    #endregion Methods
}