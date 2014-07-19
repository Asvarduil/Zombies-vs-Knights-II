using UnityEngine;
using System.Collections;

public class MinimapClick : DebuggableBehavior 
{
	#region Variables / Properties

	public float ZOffset = -10.0f;

	public bool MouseIsInMinimap
	{
		get { return _miniMapCamera.pixelRect.Contains(Input.mousePosition); }
	}

	private Camera _mainCamera;
	private Camera _miniMapCamera;
	private CameraClick _mainCameraInterface;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_mainCamera = Camera.main;
		_miniMapCamera = gameObject.camera;
		_mainCameraInterface = (CameraClick) FindObjectOfType(typeof(CameraClick));
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0) 
		    && MouseIsInMinimap)
		{ 
			RaycastHit hit;
			Ray ray = _miniMapCamera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				Vector3 newPosition = new Vector3(hit.point.x, _mainCamera.transform.position.y, hit.point.z + ZOffset);
				_mainCamera.transform.position = newPosition;
			}
		}

		// R-Click works the same as in the normal camera interface.
		if (Input.GetMouseButtonDown(1)
		    && MouseIsInMinimap)
		{
			GameObject SelectedUnit = _mainCameraInterface.Selected;
			if(SelectedUnit == null)
				return;

			RaycastHit hit;
			Ray ray = _miniMapCamera.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit))
			{
				var thing = hit.collider.gameObject;
				if(thing.tag == "Terrain")
				{
					SelectedUnit.SendMessage("NavigateTo", hit.point, SendMessageOptions.DontRequireReceiver);
				}
				else if(thing.tag == "Unit")
				{
					SelectedUnit.SendMessage("ChaseUnit", thing, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	#endregion Engine Hooks
}
