using System;
using SimpleJSON;

[Serializable]
public class AIUnitTargetingModel : IJsonSavable, ICloneable
{
    #region Fields



    #endregion Fields

    #region Methods

    public object Clone()
    {
        throw new NotImplementedException();
    }

    public JSONClass ExportState()
    {
        throw new NotImplementedException();
    }

    public void ImportState(JSONClass node)
    {
        throw new NotImplementedException();
    }

    #endregion Methods
}
