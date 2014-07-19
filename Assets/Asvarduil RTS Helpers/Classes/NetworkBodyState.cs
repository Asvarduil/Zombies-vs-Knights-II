using System;
using UnityEngine;

[Serializable]
public class NetworkBodyState
{
	#region Variables / Properties

	public double Timestamp;
	public Vector3 Position;
	public Vector3 Velocity;
	public Vector3 AngularVelocity;

	public Quaternion Rotation;

	#endregion Variables / Properties
}
