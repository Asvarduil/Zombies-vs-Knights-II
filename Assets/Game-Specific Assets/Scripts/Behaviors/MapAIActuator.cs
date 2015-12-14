using System;
using System.Collections.Generic;

public class MapAIActuator : ManagerBase<MapAIActuator>
{
    #region Variables / Properties

    public bool ExecuteAI = false;
    public Faction Faction;
    public Lockout SpawnUnitLockout;
    public Lockout TargetUnitLockout;
    public AIModel AILogic;

    private int _unitSpawnSequenceId = 0;
    //private int _unitTargetSequenceId = 0;

    private PlayerManager _player;
    private PlayerManager Player
    {
        get
        {
            if (_player == null)
                _player = PlayerManager.Instance;

            return _player;
        }
    }

    private MapController _mapController;
    private MapController MapController
    {
        get
        {
            if (_mapController == null)
                _mapController = MapController.Instance;

            return _mapController;
        }
    }

    private MatchController _matchController;
    private MatchController MatchController
    {
        get
        {
            if (_matchController == null)
                _matchController = MatchController.Instance;

            return _matchController;
        }
    }

    private AIRepository _aiRepository;
    private AIRepository AIRepository
    {
        get
        {
            if (_aiRepository == null)
                _aiRepository = AIRepository.Instance;

            return _aiRepository;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        switch(Player.Faction)
        {
            case Faction.Knights:
                Faction = Faction.Zombies;
                break;

            case Faction.Zombies:
                Faction = Faction.Knights;
                break;

            default:
                throw new InvalidOperationException("Unexpected faction: " + Player.Faction);
        }
    }

    public void Update()
    {
        if (!ExecuteAI)
            return;

        if (HasExecutedUnitSpawnCondition())
            return;

        // TODO: Get Unit Spawn AI working first...
        //if (HasExecutedUnitTargetCondition())
        //    return;
    }

    #endregion Hooks

    #region Methods

    public void LoadAI(string aiModelName)
    {
        AILogic = AIRepository.GetAIScriptByName(aiModelName);
        if (AILogic == null)
            throw new DataException("Script " + aiModelName + " does not exist in the AI Repository!");

        ExecuteAI = true;
    }

    private bool HasExecutedUnitSpawnCondition()
    {
        if (!SpawnUnitLockout.CanAttempt())
            return false;

        bool hasExecutedACondition = false;
        for(int i = 0; i < AILogic.SpawnConditions.Count; i++)
        {
            AIUnitSpawnModel current = AILogic.SpawnConditions[i];

            bool conditionMet = IsUnitSpawnConditionFulfilled(current);
            if (!conditionMet)
                continue;

            hasExecutedACondition = true;
            SpawnUnitLockout.NoteLastOccurrence();

            string unitToSpawn = current.TargetComposition[_unitSpawnSequenceId];
            FormattedDebugMessage(LogLevel.Info, "Attempting to spawn in unit {0}", unitToSpawn);
            MatchController.SpawnUnitByUnitName(unitToSpawn, Faction);
            break;
        }

        if (!hasExecutedACondition)
            DebugMessage("No unit spawn conditions were met.");

        return hasExecutedACondition;
    }

    private bool HasExecutedUnitTargetCondition()
    {
        if (!TargetUnitLockout.CanAttempt())
            return false;

        bool hasExecutedACondition = false;
        for(int i = 0; i < AILogic.TargetingConditions.Count; i++)
        {
            AIUnitTargetingModel current = AILogic.TargetingConditions[i];

            bool conditionMet = IsUnitTargetConditionFulfilled(current);
            if (!conditionMet)
                continue;

            hasExecutedACondition = true;
            TargetUnitLockout.NoteLastOccurrence();

            // TODO: Actually make a unit target some other unit.
            break;
        }

        if (!hasExecutedACondition)
            DebugMessage("No unit target conditions were met.");

        return hasExecutedACondition;
    }

    private bool IsUnitSpawnConditionFulfilled(AIUnitSpawnModel condition)
    {
        List<UnitActuator> targetPool;

        switch(condition.EvaluationPool)
        {
            case AIUnitCompEvaluationPool.All:
                targetPool = MatchController.AllUnits;
                break;

            case AIUnitCompEvaluationPool.AI:
                targetPool = MatchController.AllAIUnits;
                break;

            case AIUnitCompEvaluationPool.Player:
                targetPool = MatchController.AllPlayerUnits;
                break;

            default:
                throw new InvalidOperationException("Unexpected Evaluation Pool: " + condition.EvaluationPool);
        }

        // Reduce the target pool to a list of strings...
        List<string> targetPoolNames = new List<string>();
        for(int i = 0; i < targetPool.Count; i++)
        {
            string current = targetPool[i].Name;
            targetPoolNames.Add(current);
        }

        // Determine if the target pool contains the list of names as set in the unit spawn condition.
        bool conditionFulfilled = targetPoolNames.ContainsAll(condition.CompositionCondition);

        FormattedDebugMessage(LogLevel.Info, "Spawn condition {0} {1} fulfilled.", 
            condition.Name, 
            (conditionFulfilled ? "was" : "was not"));

        return conditionFulfilled;
    }

    private bool IsUnitTargetConditionFulfilled(AIUnitTargetingModel condition)
    {
        // TODO: Actually check conditions.
        return false;
    }

    #endregion Methods
}
