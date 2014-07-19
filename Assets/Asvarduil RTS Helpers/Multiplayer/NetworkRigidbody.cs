using System;
using UnityEngine;

public class NetworkRigidbody : DebuggableBehavior
{
	#region Variables / Properties

	public int HistoryFrames = 3;
	public float InterpolationBackTime = 0.1f;
	public float ExtrapolationLimit = 0.5f;

	private int _timestampCount;
	private NetworkBodyState _currentState;
	private NetworkBodyState[] _priorStates;
	private double _interpolationTime;
	private double _extrapolationLength;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_currentState = new NetworkBodyState();
		_priorStates = new NetworkBodyState[HistoryFrames];
	}

	public void Update()
	{
		_interpolationTime = Network.time - InterpolationBackTime;

		if(_priorStates[0] == null)
		{
			DebugMessage("There is no initial NetworkRigidbody state!");
			return;
		}

		if(_priorStates[0].Timestamp > _interpolationTime)
			ReplayPriorState();
		else
			ExtrapolateProperties();
	}

	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if(stream.isWriting)
		{
			_currentState.Position = transform.position;
			_currentState.Rotation = transform.rotation;

			stream.Serialize(ref _currentState.Position);
			stream.Serialize(ref _currentState.Rotation);
		}

		if(stream.isReading)
		{
			_currentState.Position = Vector3.zero;
			_currentState.Velocity = Vector3.zero;
			_currentState.AngularVelocity = Vector3.zero;
			_currentState.Rotation = Quaternion.identity;

			// Shift the buffer 'up', such that the final state is
			// prepared for a new state to be pushed onto the stack.
			for(int i = _priorStates.Length; i >= 1; i--)
			{
				_priorStates[i] = _priorStates[i - 1];
			}

			_currentState.Timestamp = info.timestamp;
			_priorStates[0] = _currentState;

			_timestampCount = Mathf.Min(_timestampCount - 1, _priorStates.Length);

			for(int i = 0; i < _timestampCount - 1; i++)
			{
				if(_priorStates[i].Timestamp < _priorStates[i + 1].Timestamp)
				{
					DebugMessage("State inconsistent!");
				}
			}
		}
	}

	#endregion Engine Hooks

	#region Methods

	private void ReplayPriorState()
	{
		for(int i = 0; i < _timestampCount; i++)
		{
			if(_priorStates[i].Timestamp <= _interpolationTime || i == _timestampCount - 1)
			{
				NetworkBodyState rhs = _priorStates[Mathf.Max(i - 1, 0)];
				NetworkBodyState lhs = _priorStates[i];

				double length = rhs.Timestamp - lhs.Timestamp;
				float t = 0.0f;

				if(length > 0.001f)
				{
					t = Convert.ToSingle((_interpolationTime - lhs.Timestamp) / length);
				}

				transform.localPosition = Vector3.Lerp(lhs.Position, rhs.Position, t);
				transform.localRotation = Quaternion.Slerp(lhs.Rotation, rhs.Rotation, t);
				return;
			}
		}
	}

	private void ExtrapolateProperties()
	{
		NetworkBodyState latest = _priorStates[0];
		
		_extrapolationLength = _interpolationTime - latest.Timestamp;
		if( _extrapolationLength < ExtrapolationLimit )
		{
			// TODO: Unused variable.  Determine a use for it.
			//float axisLength = Convert.ToSingle(_extrapolationLength * latest.AngularVelocity.magnitude * Mathf.Rad2Deg);
			//Quaternion angularRotation = Quaternion.AngleAxis( axisLength, latest.AngularVelocity );
			
			transform.position = latest.Position + latest.Velocity * ((float)_extrapolationLength);
			transform.rotation = latest.Rotation * latest.Rotation;
		}
	}

	#endregion Methods
}
