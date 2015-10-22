using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : ManagerBase<MapController>
{
    #region Constants

    private const string WaypointTag = "Waypoint";

    #endregion Constants

    #region Variables / Properties

    public List<Ability> UnitSpawnAbilities;
    public List<GameObject> Waypoints;
    public List<SpawnSphere> SpawnSpheres = new List<SpawnSphere>();

    private PlayerManager _player;
    public PlayerManager Player
    {
        get
        {
            if (_player == null)
                _player = PlayerManager.Instance;

            return _player;
        }
    }

    private GameUIMasterController _gui;
    private GameEventController _events;

    private UnitRepository _unit;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _player = PlayerManager.Instance;
        _gui = GameUIMasterController.Instance;
        _events = GameEventController.Instance;

        _unit = UnitRepository.Instance;

        RegisterEvents();
        UpdateWaypointList();
    }

    public void OnDestroy()
    {
        UnregisterEvents();
    }

    #endregion Hooks

    #region Game Events

    private void RegisterEvents()
    {
        _events.RegisterEventHook("SpawnUnitAtSpawnSphere", SpawnUnitAtSpawnSphereEvent);
    }

    private void UnregisterEvents()
    {
        _events.UnregisterEventHook(
            "SpawnUnitAtSpawnSphere"
        );
    }

    private IEnumerator SpawnUnitAtSpawnSphereEvent(List<string> args)
    {
        if (args.Count != 2)
            throw new ArgumentException("SpawnUnitAtSpawnSphere requires two arguments: the prefab path and the unit name.");

        string prefabPath = args[0];
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        string unitName = args[1];
        UnitModel model = _unit.GetUnitByName(unitName);

        SpawnUnitAtSpawnSphere(prefab, model);

        yield return 0;
    }

    #endregion Game Events

    #region Methods

    public List<Ability> GetUnitSpawnAbilities(Faction faction)
    {
        List<Ability> abilities = new List<Ability>();

        FormattedDebugMessage(LogLevel.Info,
            "Looking for {0} abilities, from a pool of {1} available abilities.",
            faction,
            UnitSpawnAbilities.Count);

        for(int i = 0; i < UnitSpawnAbilities.Count; i++)
        {
            Ability current = UnitSpawnAbilities[i];
            if (current.CommandTrigger != AbilityCommmandTrigger.PlayerActivated
               || current.Faction != faction)
            {
                string reason = string.Empty;
                if (current.CommandTrigger != AbilityCommmandTrigger.PlayerActivated)
                    reason = "the ability is not a Player-Activated ability.";
                else if (current.Faction != faction)
                    reason = "the ability does not belong to faction " + faction;

                FormattedDebugMessage(LogLevel.Info,
                    "Passing up ability {0}, because {1}",
                    current.Name,
                    reason);

                continue;
            }
            
            abilities.Add(current);
        }

        FormattedDebugMessage(LogLevel.Info,
            "Found {0} abilities for faction {1}.",
            abilities.Count,
            faction);

        return abilities.DeepCopyList();
    }

    public void RegisterSpawnSphere(SpawnSphere sphere)
    {
        SpawnSpheres.Add(sphere);
    }

    public void SpawnUnitAtSpawnSphere(GameObject unit, UnitModel model)
    {
        if (unit == null)
            throw new ArgumentNullException("unit");

        if (model == null)
            throw new ArgumentNullException("model");

        List<SpawnSphere> openFriendlySpheres = new List<SpawnSphere>();
        for(int i = 0; i < SpawnSpheres.Count; i++)
        {
            SpawnSphere current = SpawnSpheres[i];
            if (current.IsOccupied || current.Faction != model.Faction)
                continue;

            openFriendlySpheres.Add(current);
        }

        if (openFriendlySpheres.Count == 0
           && model.Faction == _player.Faction)
        {
            // TODO: Audio cue that the player can't do this right now...
            DebugMessage("Cannot spawn unit; there are no open Spawn Spheres...");
            _gui.PresentTooltip("Not enough places to spawn a unit at...");
            return;
        }

        openFriendlySpheres[0].SpawnUnitFromModel(unit, model);
    }

    public GameObject FindNearestWaypoint(Vector3 position, GameObject excludedWaypoint = null)
    {
        GameObject result = null;

        float currentClosest = 1000.0f;
        for(int i = 0; i < Waypoints.Count; i++)
        {
            GameObject current = Waypoints[i];

            if (excludedWaypoint == current)
                continue;

            float currentDistance = Vector3.Distance(position, current.transform.position);
            if (currentDistance >= currentClosest)
                continue;

            currentClosest = currentDistance;
            result = current;
        }

        return result;
    }

    private void UpdateWaypointList()
    {
        Waypoints = new List<GameObject>();

        var waypoints = GameObject.FindGameObjectsWithTag(WaypointTag);
        for (int i = 0; i < waypoints.Length; i++)
        {
            GameObject current = waypoints[i];
            Waypoints.Add(current);
        }
    }

    #endregion Methods
}
