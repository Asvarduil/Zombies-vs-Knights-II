using System;
using UnityEngine;

public class ResourceSpawnSphere : DebuggableBehavior
{
    #region Variables / Properties

    public Lockout SpawnLockout;
    public string PickupPrototypePath;
    public string ResourceModelName;

    private GameObject _lastSpawnedPickup = null;

    private PickupRepository _pickup;
    private PickupRepository Pickup
    {
        get
        {
            if (_pickup == null)
                _pickup = PickupRepository.Instance;

            return _pickup;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Update()
    {
        CheckForSpawn();
    }

    #endregion Hooks

    #region Methods

    public void CheckForSpawn()
    {
        if (!SpawnLockout.CanAttempt())
            return;

        // TODO: I suspect this will throw exceptions when linked to a stale reference...
        if (_lastSpawnedPickup != null)
            return;

        if (string.IsNullOrEmpty(PickupPrototypePath))
            throw new InvalidOperationException("Cannot create a pickup based on an empty prototype.");

        GameObject pickup = Resources.Load<GameObject>(PickupPrototypePath);
        PickupModel model = Pickup.GetPickupByName(ResourceModelName);

        _lastSpawnedPickup = SpawnPickupFromModel(pickup, model);
    }

    public GameObject SpawnPickupFromModel(GameObject unit, PickupModel model)
    {
        if (unit == null)
            throw new ArgumentNullException("unit");

        if (model == null)
            throw new ArgumentNullException("model");

        GameObject generatedPickup = (GameObject) Instantiate(unit, transform.position, transform.rotation);
        generatedPickup.name = model.Name;
        DebugMessage("Created pickup " + generatedPickup.name);

        ResourcePickupActuator actuator = generatedPickup.GetComponent<ResourcePickupActuator>();
        if (actuator == null)
            throw new InvalidOperationException("There is no unit actuator for pickup " + generatedPickup.name);

        DebugMessage("Realizing model on pickup " + generatedPickup.name);
        actuator.RealizeModel(model);

        return generatedPickup;
    }

    #endregion Methods
}
