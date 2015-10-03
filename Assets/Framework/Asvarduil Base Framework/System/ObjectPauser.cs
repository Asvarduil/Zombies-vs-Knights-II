using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPauser : DebuggableBehavior
{
    #region Variables

    private List<MonoBehaviour> _components;

    #endregion Variables

    #region Hooks

    public void Start()
    {
        _components = GetObjectBehaviors();
    }

    #endregion Hooks

    #region Methods

    public void Resume()
    {
        for(int i = 0; i < _components.Count; i++)
        {
            MonoBehaviour current = _components[i];
            DebugMessage("Resuming component " + current.name + "...");

            try
            {
                ISuspendable component = (ISuspendable)current;
                component.Resume();
            }
            catch (InvalidCastException)
            {
                // Swallow; don't call the behavior.
                DebugMessage("Component " + current.name + " does not support resuming.");
            }
        }
    }

    public void Suspend()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            MonoBehaviour current = _components[i];
            DebugMessage("Suspending component " + current.name + "...");

            try
            {
                ISuspendable component = (ISuspendable)current;
                component.Suspend();
            }
            catch(InvalidCastException)
            {
                // Swallow; don't call the behavior.
                DebugMessage("Component " + current.name + " does not support suspending.");
            }
        }
    }

    private List<MonoBehaviour> GetObjectBehaviors()
    {
        List<MonoBehaviour> results = new List<MonoBehaviour>();
        MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
        for(int i = 0; i < components.Length; i++)
        {
            MonoBehaviour current = components[i];
            results.Add(current);
        }

        DebugMessage("Detected " + results.Count + " behaviors on this game object.");
        return results;
    }

    #endregion Methods
}
