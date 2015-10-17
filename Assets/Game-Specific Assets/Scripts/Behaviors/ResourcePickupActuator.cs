using UnityEngine;

public class ResourcePickupActuator : DebuggableBehavior
{
    #region Variables / Properties

    public int ResourceYield = 10;

    private MatchController _match;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _match = MatchController.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        UnitActuator actuator = who.GetComponent<UnitActuator>();
        if (actuator == null)
            return;

        Faction awardedFaction = actuator.Faction;
        // TODO: Implement!
        //_match.AwardResources(awardedFaction, ResourceYield);
    }

    #endregion Hooks

    #region Methods

    // TODO: Create a pickup model to realize.
    public void RealizeModel()
    {

    }

    #endregion Methods
}
