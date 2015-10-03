using System.Collections.Generic;
using UnityEngine;

public class MapController : ManagerBase<MapController>
{
    #region Constants

    private const string WaypointTag = "Waypoint";

    #endregion Constants

    #region Variables / Properties

    public List<GameObject> Waypoints;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        UpdateWaypointList();
    }

    #endregion Hooks

    #region Methods

    public GameObject FindNearestWaypoint(Vector3 position)
    {
        GameObject result = null;

        float currentClosest = 1000.0f;
        for(int i = 0; i < Waypoints.Count; i++)
        {
            GameObject current = Waypoints[i];
            float currentDistance = Vector3.Distance(position, current.transform.position);
            if (currentDistance >= currentClosest)
                continue;

            currentClosest = currentDistance;
            result = current;
        }

        return result;
    }

    private void UpdateWaypointList()
    {
        Waypoints = new List<GameObject>();

        var waypoints = GameObject.FindGameObjectsWithTag(WaypointTag);
        for (int i = 0; i < waypoints.Length; i++)
        {
            GameObject current = waypoints[i];
            Waypoints.Add(current);
        }
    }

    #endregion Methods
}
