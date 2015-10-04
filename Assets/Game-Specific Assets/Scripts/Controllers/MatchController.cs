using System;
using System.Collections.Generic;

public enum ActivityType
{
    Suspend,
    Resume
}

public class MatchController : ManagerBase<MatchController>
{
    #region Variables / Properties

    private PlayerManager _player;
    private GameUIMasterController _gameUI;
    private UnitSelectionManager _selection;
    private GameEventController _events;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _player = PlayerManager.Instance;
        _gameUI = GameUIMasterController.Instance;
        _selection = UnitSelectionManager.Instance;
        _events = GameEventController.Instance;
    }

    #endregion Hooks

    #region Methods

    public void UseSelectedUnitAbility(int abilityIndex)
    {
        Ability ability = _selection.SelectedUnit.Abilities[abilityIndex];

        // TODO: Check resources.
        // TODO: Instantiate effect on the field.

        _events.RunGameEventGroup(ability.GameEvents);
    }

    public void CheckForMatchConclusion()
    {
        var keyUnits = GetKeyUnits();

        bool playerWins = GetKeyUnitsForFaction(_player.Faction, keyUnits).Count > 0;
        bool playerLost = GetKeyUnitsForFaction(_player.Faction, keyUnits).Count == 0;

        // Determine match state...
        MatchState state = MatchState.OnGoing;
        if (playerWins)
            state = MatchState.Victory;
        else if (playerLost)
            state = MatchState.Lost;

        RadiateActivityCommand(ActivityType.Suspend);
        _player.RecordMatchOutcome(state);
        _gameUI.ShowMatchOutcome(state);
    }

    private List<UnitActuator> GetKeyUnits()
    {
        var result = new List<UnitActuator>();

        var allUnits = FindObjectsOfType<UnitActuator>();
        for(int i = 0; i < allUnits.Length; i++)
        {
            UnitActuator current = allUnits[i];
            if (!current.IsKeyUnit)
                continue;

            if (current.IsDead)
                continue;

            result.Add(current);
        }

        return result;
    }

    private List<UnitActuator> GetKeyUnitsForFaction(Faction faction, List<UnitActuator> sourceList)
    {
        var result = new List<UnitActuator>();

        for (int i = 0; i < sourceList.Count; i++)
        {
            UnitActuator current = sourceList[i];
            if (current.Faction != faction)
                continue;

            if (current.IsDead)
                continue;

            result.Add(current);
        }

        return result;
    }

    private void RadiateActivityCommand(ActivityType activity)
    {
        var pausableGameObjects = FindObjectsOfType<ObjectPauser>();
        if (pausableGameObjects.Length == 0)
            throw new InvalidOperationException("There are no Pausable Objects in the scene!");

        switch (activity)
        {
            case ActivityType.Suspend:
                for (int i = 0; i < pausableGameObjects.Length; i++)
                {
                    ObjectPauser current = pausableGameObjects[i];
                    current.Suspend();
                }
                break;

            case ActivityType.Resume:
                for (int i = 0; i < pausableGameObjects.Length; i++)
                {
                    ObjectPauser current = pausableGameObjects[i];
                    current.Resume();
                }
                break;
        }
    }

    #endregion Methods
}


    