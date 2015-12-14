using UnityEngine;

public class TargetingWhoosh : DebuggableBehavior
{
    #region Variables / Properties

    public float Speed;
    public float TurnSpeed;
    public float PathingError;
    public float TargetTouchDistance;
    public GameObject Target;

    private bool _touchedTarget = false;

    #endregion Variables / Properties

    #region Hooks

    public void Update()
    {
        AdvanceWhoosh();
    }

    #endregion Hooks

    #region Methods

    public void ReadyWhoosh(GameObject target)
    {
        Target = target;
    }

    public void AdvanceWhoosh()
    {
        if (_touchedTarget)
            return;

        RotateTowards(Target.transform.position);
        MoveTowards(Target.transform.position);  

        _touchedTarget = Vector3.Distance(transform.position, Target.transform.position) < TargetTouchDistance;
    }

    private void RotateTowards(Vector3 point)
    {
        var direction = point - transform.position;
        if (direction.magnitude < PathingError)
            return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void MoveTowards(Vector3 point)
    {
        Vector3 moveDirection = Vector3.forward * (Speed * Time.deltaTime);
        transform.Translate(moveDirection);
    }

    #endregion Methods
}
