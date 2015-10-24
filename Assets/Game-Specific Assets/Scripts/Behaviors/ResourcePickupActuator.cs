using System;
using UnityEngine;

public class ResourcePickupActuator : DebuggableBehavior
{
    #region Variables / Properties

    public string Name = string.Empty;
    public int ResourceYield = 0;
    public Vector3 RotationRate = Vector3.zero;
    public GameObject PickupMeshObject;

    private string _meshPath;

    private UnitSelectionManager _selection;
    private UnitSelectionManager Selection
    {
        get
        {
            if (_selection == null)
                _selection = UnitSelectionManager.Instance;

            return _selection;
        }
    }

    private MatchController _match;
    private MatchController Match
    {
        get
        {
            if (_match == null)
                _match = MatchController.Instance;

            return _match;
        }
    }

    private GameEventController _gameEvents;
    private GameEventController GameEvents
    {
        get
        {
            if (_gameEvents == null)
                _gameEvents = GameEventController.Instance;

            return _gameEvents;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _selection = UnitSelectionManager.Instance;
        _match = MatchController.Instance;
        _gameEvents = GameEventController.Instance;
    }

    public void Update()
    {
        transform.Rotate(RotationRate * Time.deltaTime);
    }

    public void OnMouseEnter()
    {
        _selection.UpdateCursor(gameObject);
    }

    public void OnTriggerEnter(Collider who)
    {
        UnitActuator actuator = who.GetComponent<UnitActuator>();
        if (actuator == null)
            return;

        Faction awardedFaction = actuator.Faction;
        Match.AwardResourcesToFaction(awardedFaction, ResourceYield);
        actuator.IssueCommand(AbilityCommmandTrigger.Defend);

        Destroy(gameObject);
    }

    #endregion Hooks

    #region Methods

    public void RealizeModel(PickupModel model)
    {
        if (model == null)
            throw new ArgumentNullException("model");

        Name = model.Name;
        RotationRate = model.RotationRate;
        ResourceYield = model.ResourceYield;

        _meshPath = model.MeshPath;
        if (! string.IsNullOrEmpty(_meshPath))
        {
            GameObject unitMesh = Resources.Load<GameObject>(_meshPath);

            PickupMeshObject = (GameObject)Instantiate(unitMesh, transform.position, transform.rotation);
            PickupMeshObject.name = "Mesh";
            PickupMeshObject.transform.parent = gameObject.transform;

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.enabled = true;
            collider.convex = true;
            collider.isTrigger = true;
            collider.sharedMesh = PickupMeshObject.GetComponent<MeshFilter>().mesh;
        }

        GameEvents.RunGameEventGroup(model.GameEvents);
    }

    #endregion Methods
}
