using UnityEngine;

public class ResourcePickupActuator : DebuggableBehavior
{
    #region Variables / Properties

    public string Name = string.Empty;
    public int ResourceYield = 0;
    public GameObject PickupMeshObject;

    private string _meshPath;

    private MatchController _match;
    private GameEventController _gameEvents;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _match = MatchController.Instance;
        _gameEvents = GameEventController.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        UnitActuator actuator = who.GetComponent<UnitActuator>();
        if (actuator == null)
            return;

        Faction awardedFaction = actuator.Faction;
        _match.AwardResourcesToFaction(awardedFaction, ResourceYield);
    }

    #endregion Hooks

    #region Methods

    public void RealizeModel(PickupModel model)
    {
        Name = model.Name;
        ResourceYield = model.ResourceYield;

        _meshPath = model.MeshPath;
        if (string.IsNullOrEmpty(_meshPath))
        {
            GameObject unitMesh = Resources.Load<GameObject>(_meshPath);

            PickupMeshObject = (GameObject)Instantiate(unitMesh, transform.position, transform.rotation);
            PickupMeshObject.name = "Mesh";
            PickupMeshObject.transform.parent = gameObject.transform;

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.enabled = true;
            collider.convex = true;
            collider.sharedMesh = PickupMeshObject.GetComponent<MeshFilter>().mesh;
        }

        _gameEvents.RunGameEventGroup(model.GameEvents);
    }

    #endregion Methods
}
