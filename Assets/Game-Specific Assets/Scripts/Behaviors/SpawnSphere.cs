using System;
using UnityEngine;

public class SpawnSphere : DebuggableBehavior
{
    #region Variables / Properties

    public Faction Faction;
    public int OccupyCount = 0;

    public bool IsOccupied
    {
        get { return OccupyCount > 0; }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        MapController controller = MapController.Instance;
        controller.RegisterSpawnSphere(this);
    }

    public void OnTriggerEnter(Collider who)
    {
        UnitActuator unit = who.gameObject.GetComponent<UnitActuator>();
        if (unit == null)
            return;

        OccupyCount++;
    }

    public void OnTriggerExit(Collider who)
    {
        UnitActuator unit = who.gameObject.GetComponent<UnitActuator>();
        if (unit == null)
            return;

        OccupyCount--;
    }

    #endregion Hooks

    #region Methods

    public void SpawnUnitFromModel(GameObject unit, UnitModel model)
    {
        if (unit == null)
            throw new ArgumentNullException("unit");

        if (model == null)
            throw new ArgumentNullException("model");

        GameObject physicalUnit = (GameObject)Instantiate(unit, transform.position, transform.rotation);
        physicalUnit.name = model.Name;

        UnitActuator actuator = physicalUnit.GetComponent<UnitActuator>();
        if (actuator == null)
            throw new InvalidOperationException("There is no unit actuator for unit " + physicalUnit.name);

        actuator.RealizeModel(model);

        OccupyCount++;
    }

    #endregion Methods
}
