using UnityEngine;
using System.Collections;

public class CameraScroll : DebuggableBehavior
{
	#region Variables / Properties

	public float ScrollSpeed = 1.0f;
	public float ScrollMargin = 0.05f;

	public bool HangingLeft
	{
		get 
		{ 
			return Input.mousePosition.x < (int)(Screen.width * ScrollMargin)
			         && Input.mousePosition.x > -1; 
		}
	}

	public bool HangingRight
	{
		get 
		{ 
			return Input.mousePosition.x > Screen.width - (int)(Screen.width * ScrollMargin)
				   && Input.mousePosition.x < Screen.width + 1; 
		}
	}

	public bool HangingTop
	{
		get 
		{ 
			return Input.mousePosition.y < (int)(Screen.height * ScrollMargin)
				   && Input.mousePosition.y > -1; 
		}
	}

	public bool HangingBottom
	{
		get 
		{ 
			return Input.mousePosition.y > Screen.height - (int)(Screen.height * ScrollMargin)
				   && Input.mousePosition.y < Screen.height + 1; 
		}
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Update()
	{
		// TODO: Make it so that the mouse being in the corner causes movement in
		//       both the X and Z dimensions at once.
		if(HangingLeft)
			rigidbody.MovePosition(transform.position + Vector3.left * ScrollSpeed * Time.deltaTime);

		if(HangingRight)
			rigidbody.MovePosition(transform.position + Vector3.right * ScrollSpeed * Time.deltaTime);

		if(HangingBottom)
			rigidbody.MovePosition(transform.position + Vector3.forward * ScrollSpeed * Time.deltaTime);

		if(HangingTop)
			rigidbody.MovePosition(transform.position + -Vector3.forward * ScrollSpeed * Time.deltaTime);
	}

	#endregion Engine Hooks

	#region Methods

	#endregion Methods
}
