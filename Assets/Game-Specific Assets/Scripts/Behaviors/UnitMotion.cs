using System;
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

        Vector3 direction;
        try
        {
            direction = transform.position - Target.transform.position;
        }
        catch (MissingReferenceException)
        {
            FormattedDebugMessage(
                LogLevel.Warn,
                "Unit {0}'s target no longer exists; halting instead.",
                name
            );

            Target = null;
            Halt();
            return false;
        }

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
            return;
        }

        GameObject waypoint = _map.FindNearestWaypoint(transform.position, _currentWaypointObject);
        if(waypoint == null)
        {
            _currentWaypoint = Target.transform.position;
            _currentWaypointObject = Target;
            return;
        }

        _currentWaypoint = waypoint.transform.position;
        _currentWaypointObject = waypoint;
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

        if (Target == null)
        {
            Halt();
            return transform.position;
        }

        try
        {
            Vector3 destination = Target.transform.position;
            return destination;
        }
        catch (MissingReferenceException)
        {
            FormattedDebugMessage(
                LogLevel.Warn,
                "Unit {0}'s target no longer exists; halting, and setting current position as destination to move to.",
                name
            );

            Halt();
            return transform.position;
        }
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
