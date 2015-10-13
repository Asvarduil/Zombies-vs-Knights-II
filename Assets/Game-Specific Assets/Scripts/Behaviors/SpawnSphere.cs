using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSphere : DebuggableBehavior
{
    #region Variables / Properties

    public Faction Faction;
    public int OccupyCount = 0;
    public List<GameObject> OccupyingObjects = new List<GameObject>();

    public bool IsOccupied
    {
        get { return OccupyCount > 0; }
    }

    private MatchController _match;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _match = MatchController.Instance;

        MapController controller = MapController.Instance;
        controller.RegisterSpawnSphere(this);
    }

    public void OnTriggerEnter(Collider who)
    {
        UnitActuator unit = who.gameObject.GetComponent<UnitActuator>();
        if (unit == null)
            return;

        DebugMessage("Game Object " + who.name + " has entered spawn sphere " + gameObject.name);
        AddOccupyingObject(who.gameObject);
    }

    public void OnTriggerExit(Collider who)
    {
        UnitActuator unit = who.gameObject.GetComponent<UnitActuator>();
        if (unit == null)
            return;

        DebugMessage("Game Object " + who.name + " has left spawn sphere " + gameObject.name);
        RemoveOccupyingObject(who.gameObject);
    }

    #endregion Hooks

    #region Methods

    public void AddOccupyingObject(GameObject who)
    {
        DebugMessage("Adding game object " + who.name + " as an occupying object.");

        OccupyingObjects.Add(who);
        OccupyCount++;
    }

    public void RemoveOccupyingObject(GameObject who)
    {
        DebugMessage("Removing game object " + who.name + " as an occupying object.");

        OccupyingObjects.Remove(who);
        OccupyCount--;
    }

    public void SpawnUnitFromModel(GameObject unit, UnitModel model)
    {
        if (unit == null)
            throw new ArgumentNullException("unit");

        if (model == null)
            throw new ArgumentNullException("model");

        GameObject physicalUnit = (GameObject)Instantiate(unit, transform.position, transform.rotation);
        physicalUnit.name = model.Name;
        DebugMessage("Created physical unit " + physicalUnit.name);

        UnitActuator actuator = physicalUnit.GetComponent<UnitActuator>();
        if (actuator == null)
            throw new InvalidOperationException("There is no unit actuator for unit " + physicalUnit.name);

        DebugMessage("Realizing model " + model.Name + " on game object " + physicalUnit.name);
        actuator.RealizeModel(model);

        // Set the opposing Key Unit as the unit's default target.
        UnitMotion motion = physicalUnit.GetComponent<UnitMotion>();
        if (motion != null)
        {
            GameObject newUnitTarget = _match.GetFirstOpposingKeyUnit(actuator.Faction).gameObject;
            motion.SetTarget(newUnitTarget);
        }
    }

    #endregion Methods
}
