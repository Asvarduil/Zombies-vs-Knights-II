using UnityEngine;

public class UnitMotion : DebuggableBehavior, ISuspendable
{
    #region Variables / Properties

    public bool IsMoving = false;
    public bool IsFindingClearPath = false;
    public float PathingError = 0.01f;
    public float CloseEnoughDistance = 2.0f;
    public GameObject Target;

    private float _moveSpeed;
    private float _turnSpeed;
    private ModifiableStat _speedStat;
    private ModifiableStat _turnStat;

    private Vector3 _currentWaypoint;
    private GameObject _currentWaypointObject = null;

    private Collider _collider;
    private UnitActuator _unit;
    private MapController _map;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _map = MapController.Instance;

        _collider = GetComponent<Collider>();
        _unit = GetComponent<UnitActuator>();
        _speedStat = _unit.Stats.FindItemByName("MoveSpeed");
        _turnStat = _unit.Stats.FindItemByName("TurnSpeed");
    }

    public void FixedUpdate()
    {
        if (!IsMoving)
            return;

        if (!IsGrounded())
            return;

        FetchMovementSpeeds();
        Vector3 destination = EvaluateCurrentDestination();

        RotateTowards(destination);
        MoveTowards(destination);
    }

    #endregion Hooks

    #region Methods

    public void Suspend()
    {
        Halt();
    }

    public void Resume()
    {
        IsMoving = true;
    }

    public void Halt()
    {
        IsMoving = false;
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
        IsMoving = true;
        SetCurrentDestination();
    }

    private bool IsNearCurrentWaypoint()
    {
        if (! IsFindingClearPath)
            return false;

        return Mathf.Abs(Vector3.Distance(transform.position, _currentWaypoint)) <= CloseEnoughDistance;
    }

    private bool HasPathToTarget()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - Target.transform.position;
        if (!Physics.Raycast(transform.position, direction, out hit))
            return false;
        
        return hit.collider.gameObject != Target;
    }

    private bool IsGrounded()
    {
        Vector3 center = _collider.bounds.center;

        Vector3 floorDisplacement = center;
        floorDisplacement.y = center.y - 0.1f;
        float radius = _collider.bounds.size.x / 2;

        return Physics.CheckCapsule(center, floorDisplacement, radius);
    }

    private void FetchMovementSpeeds()
    {
        _moveSpeed = _speedStat.ModifiedValue;
        _turnSpeed = _turnStat.ModifiedValue;
    }

    private void SetCurrentDestination()
    {
        IsFindingClearPath = HasPathToTarget();
        if (!IsFindingClearPath)
        {
            DebugMessage("Has direct path to target unit.");
            return;
        }

        GameObject waypoint = _map.FindNearestWaypoint(transform.position, _currentWaypointObject);
        if(waypoint == null)
        {
            DebugMessage("No direct path, but no waypoint!  Heading for target anyways...", LogLevel.Warning);
            _currentWaypoint = Target.transform.position;
            _currentWaypointObject = Target;
            return;
        }

        _currentWaypoint = waypoint.transform.position;
        _currentWaypointObject = waypoint;

        DebugMessage("No direct path to " + Target.name + "; moving to " + waypoint.name + " first.");
    }

    private Vector3 EvaluateCurrentDestination()
    {
        if (IsNearCurrentWaypoint())
        {
            DebugMessage("Close enough to a waypoint.  Looking to see if we can proceed to target...");
            SetCurrentDestination();
        }

        if (IsFindingClearPath)
        {
            return _currentWaypoint;
        }

        return Target.transform.position;
    }

    private void RotateTowards(Vector3 point)
    {
        var direction = point - transform.position;
        if (direction.magnitude < PathingError)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), _turnSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void MoveTowards(Vector3 point)
    {
        Vector3 moveDirection = Vector3.forward * (_moveSpeed * Time.deltaTime);
        transform.Translate(moveDirection);
    }

    #endregion Methods
}
