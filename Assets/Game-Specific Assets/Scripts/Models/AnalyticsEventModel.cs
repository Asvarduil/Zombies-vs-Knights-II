using System;
using SimpleJSON;

[Serializable]
public class AnalyticsEventModel : IJsonSavable
{
    #region Fields

    public string SessionID;
    public string EventType;
    public string EventDescription;

    #endregion Fields

    #region Methods

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["SessionID"] = SessionID;
        state["EventType"] = EventType;
        state["EventDescription"] = EventDescription;

        return state;
    }

    public void ImportState(JSONClass node)
    {
        // TODO: Determine whether this is even necessary.
    }

    #endregion Methods
}

