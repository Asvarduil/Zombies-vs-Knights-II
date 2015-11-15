using System;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class MaterialModel : IJsonSavable, ICloneable
{
    #region Fields

    public string MaterialPath;
    public string DiffuseTexturePath;
    public string BumpTexturePath;
    public string LightTexturePath;
    public Vector2 Tiling;

    #endregion Fields

    #region Methods

    public object Clone()
    {
        MaterialModel clone = new MaterialModel
        {
            MaterialPath = MaterialPath,
            DiffuseTexturePath = DiffuseTexturePath,
            BumpTexturePath = BumpTexturePath,
            LightTexturePath = LightTexturePath,
            Tiling = Tiling
        };

        return clone;
    }

    public void ImportState(JSONClass node)
    {
        MaterialPath = node["MaterialPath"];
        DiffuseTexturePath = node["DiffuseTexturePath"];
        BumpTexturePath = node["BumpTexturePath"];
        LightTexturePath = node["LightTexturePath"];
        Tiling = node["Tiling"].ImportVector2();
    }

    public JSONClass ExportState()
    {
        throw new InvalidOperationException("MaterialModels are read-only.");
    }

    #endregion Methods
}
