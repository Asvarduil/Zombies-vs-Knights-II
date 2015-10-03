﻿using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class GameEvent : ICloneable, IJsonSavable
{
    #region Variables / Properties

    public string Event;
    public List<string> EventArgs;

    #endregion Variables / Properties

    #region Constructors

    public GameEvent()
    {

    }

    public GameEvent(JSONClass state)
    {
        ImportState(state);
    }

    #endregion Constructors

    #region Methods

    public object Clone()
    {
        var clone = new GameEvent
        {
            Event = Event,
            EventArgs = new List<string>()
        };

        for (int i = 0; i < EventArgs.Count; i++)
        {
            clone.EventArgs.Add(EventArgs[i]);
        }

        return clone;
    }

    public void ImportState(JSONClass state)
    {
        Event = state["Event"];
        EventArgs = state["EventArgs"].AsArray.UnfoldStringJsonArray();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Event"] = new JSONData(Event);
        state["EventArgs"] = EventArgs.FoldPrimitiveList();

        return state;
    }

    #endregion Methods
}
