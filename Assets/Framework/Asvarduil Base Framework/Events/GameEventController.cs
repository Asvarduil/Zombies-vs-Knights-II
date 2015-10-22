using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventController : ManagerBase<GameEventController>
{
    #region Variables / Properties

    private List<GameEventHook> _eventFunctions;

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _eventFunctions = new List<GameEventHook>();

        RegisterEvents();
    }

    #endregion Hooks

    #region Events

    public void RegisterEvents()
    {
        RegisterEventHook("DebugMessage", DebugMessageEvent);
    }

    /// <summary>
    /// Allows the Game Event Controller to interpret an instruction to print a message to the console.
    /// </summary>
    /// <param name="args">A list of up to two strings.  The first arg is a message to print; the second is an optional LogLevel.</param>
    /// <returns>IEnumerator, allowing this instruction to run concurrently with others, or in sequence.</returns>
    public IEnumerator DebugMessageEvent(List<string> args)
    {
        if (args.IsNullOrEmpty())
            throw new ArgumentException("DebugMessage requires at least one argument, a message.");

        string message = args[0];
        LogLevel logLevel = LogLevel.Info;

        if(args.Count == 2)
        {
            logLevel = (LogLevel) Enum.Parse(typeof(LogLevel), args[1]);
        }

        DebugMessage(message, logLevel);

        yield return 0;
    }

    #endregion Events

    #region Methods

    public void RegisterEventHook(string eventName, Func<List<string>, IEnumerator> eventFunction)
    {
        GameEventHook newHook = new GameEventHook
        {
            Name = eventName,
            Function = eventFunction
        };

        _eventFunctions.Add(newHook);
        DebugMessage("Registered event '" + eventName + "'.");
    }

    public void UnregisterEventHook(params string[] eventNames)
    {
        for (int i = 0; i < eventNames.Length; i++)
        {
            string current = eventNames[i];

            for (int j = 0; j < _eventFunctions.Count; j++)
            {
                GameEventHook currentEvent = _eventFunctions[j];
                if (currentEvent.Name != current)
                    continue;

                DebugMessage("Unregistering event " + current);
                _eventFunctions.Remove(currentEvent);
                break;
            }
        }
    }

    // Immediately runs all game events simultaneously; the first and second events run at the same time.
    public void RunGameEventGroup(List<GameEvent> gameEvents)
    {
        DebugMessage("Controller is executing a game event group with " + gameEvents.Count + " members...");

        for (int i = 0; i < gameEvents.Count; i++)
        {
            GameEvent gameEvent = gameEvents[i];
            StartCoroutine(RunGameEvent(gameEvent));
        }
    }

    // Runs each game event sequentially; the second event doesn't run until the first is done.
    public IEnumerator RunGameEventSequence(List<GameEvent> gameEvents)
    {
        DebugMessage("Controller is executing a game event group with " + gameEvents.Count + " members...");

        for (int i = 0; i < gameEvents.Count; i++)
        {
            GameEvent gameEvent = gameEvents[i];
            yield return StartCoroutine(RunGameEvent(gameEvent));
        }
    }

    public IEnumerator RunGameEvent(GameEvent gameEvent)
    {
        DebugMessage("Controller is executing game event: " + gameEvent.Event + "...");

        string eventName = gameEvent.Event;
        List<string> eventArgs = gameEvent.EventArgs;

        // Find the first coroutine on the child behaviors with a name that matches the event name.
        GameEventHook coroutine = _eventFunctions.FirstOrDefault(f => f.Name == eventName);
        if (coroutine == default(GameEventHook))
        {
            Debug.LogError("Could not find an event named " + eventName + " in the registered event list.");
            yield break;
        }

        DebugMessage(eventName + " is registered!  Doing it.");
        yield return StartCoroutine(coroutine.Function(eventArgs));
    }

    #endregion Methods
}
