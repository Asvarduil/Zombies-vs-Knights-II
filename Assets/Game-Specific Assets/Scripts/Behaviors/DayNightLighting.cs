using UnityEngine;
using System.Collections;

public class DayNightLighting : MonoBehaviour, ISuspendable
{
    #region Variables / Properties

    public bool IsTimeAdvancing = true;
    public Vector3 Axis;
    public float DayPeriod;

    #endregion Variables / Properties

    #region Hooks

    public void FixedUpdate()
    {
        if (!IsTimeAdvancing)
            return;

        Vector3 rotationStep = Axis / DayPeriod;
        transform.Rotate(rotationStep);
    }

    #endregion Hooks

    #region Methods

    public void Suspend()
    {
        IsTimeAdvancing = false;
    }

    public void Resume()
    {
        IsTimeAdvancing = true;
    }

    #endregion Methods
}
