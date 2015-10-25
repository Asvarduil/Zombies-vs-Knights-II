using System;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class PositionRotationPair : IJsonSavable, ICloneable
{
    #region Fields

    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;

    #endregion Fields

    #region Methods

    public object Clone()
    {
        PositionRotationPair clone = new PositionRotationPair
        {
            Position = Position,
            Rotation = Rotation,
            Scale = Scale
        };

        return clone;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Position"] = Position.ExportAsJson();
        state["Rotation"] = Rotation.ExportAsJson();
        state["Scale"] = Scale.ExportAsJson();

        return state;
    }

    public void ImportState(JSONClass node)
    {
        Position = node["Position"].AsObject.ImportVector3();
        Rotation = node["Rotation"].AsObject.ImportVector3();
        Scale = node["Scale"].AsObject.ImportVector3();
    }

    #endregion Methods
}

