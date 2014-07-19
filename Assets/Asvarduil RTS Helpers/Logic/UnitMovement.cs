using UnityEngine;
using System.Collections;

public class UnitMovement : DebuggableBehavior
{
	#region Variables / Properties

	public bool CanMove = true;
	public float MoveSpeed = 5.0f;
	public float TurnSpeed = 1.0f;
	public float NavigationMargin = 1.0f;

	private bool _haltMovement = true;
	private Vector3 _moveTarget;
	private GameObject _attackTarget;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{

	}

	public void Update()
	{
		if(! CanMove)
			return;

		if(_haltMovement)
			return;

		if(_attackTarget != null)
		{
			_moveTarget = _attackTarget.transform.position;
		}

		MoveTowards(_moveTarget);
		CheckAtDestination();
	}

	#endregion Engine Hooks

	#region Methods

	public void ChaseUnit(GameObject unit)
	{
		// Don't chase yourself.  That's just dumb.
		if(unit == gameObject)
			return;

		_haltMovement = false;
		_attackTarget = unit;
	}

	public void NavigateTo(Vector3 point)
	{
		_haltMovement = false;
		_attackTarget = null;
		_moveTarget = point;
	}

	public void CheckAtDestination()
	{
		_haltMovement = Vector3.Distance(transform.position, _moveTarget) < NavigationMargin;
	}

	private void MoveTowards(Vector3 point)
	{
		RotateTowards(point);

		Vector3 direction = Vector3.forward * (MoveSpeed * Time.deltaTime);
		transform.Translate(direction);
	}

	private void RotateTowards(Vector3 point)
	{
		Vector3 direction = point - transform.position;
		if( direction.magnitude < NavigationMargin ) 
			return;
		
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.deltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}

	#endregion Methods
}
