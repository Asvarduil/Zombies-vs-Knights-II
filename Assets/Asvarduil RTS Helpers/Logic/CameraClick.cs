using UnityEngine;
using System.Collections;

public class CameraClick : DebuggableBehavior
{
	#region Variables / Properties

	public bool Enabled = true;
	public float ClickCastDistance = 1000;

	private GameObject _selected = null;
	private TeamManager _teamManager;
	private MinimapClick _miniMapCamera;
	private PlayerGUIManager _playerGUIManager;

	public GameObject Selected
	{
		get { return _selected; }
	}

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_teamManager = TeamManager.Instance;
		_playerGUIManager = PlayerGUIManager.Instance;

		_miniMapCamera = (MinimapClick) FindObjectOfType(typeof(MinimapClick));
	}

	public void Update()
	{
		if(! Enabled)
			return;

		// LMB - Select Unit
		if(Input.GetMouseButton(0)
		   && ! _miniMapCamera.MouseIsInMinimap
		   && ! _playerGUIManager.MouseIsInCommandGUI)
		{
			Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			GameObject what;

			if(Physics.Raycast(clickRay, out hit, ClickCastDistance))
			{
				what = hit.collider.gameObject;

				if(_selected != null)
					_selected.SendMessage("DeselectMe", SendMessageOptions.DontRequireReceiver);

				// TODO: Ditto.
				Teams myTeam = _teamManager.PlayerTeam;
				what.SendMessage("SelectMe", myTeam,  SendMessageOptions.DontRequireReceiver);
				_selected = what;
			}
		}

		if(Input.GetMouseButton(1)
		   && ! _miniMapCamera.MouseIsInMinimap)
		{
			// R-Click actions are disallowed when spectating.
			if(_teamManager.PlayerTeam == Teams.Spectator)
				return;

			// R-Click is only usable when a unit is selected.
			if(_selected == null)
				return;

			// Only issue movement commands if the unit is friendly to you.
			// Neutral and Enemy units should not react to your commands.
			UnitFaction faction = _selected.GetComponent<UnitFaction>();
			if(faction.Team != _teamManager.PlayerTeam)
				return;

			Ray clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			GameObject thing;

			if(Physics.Raycast(clickRay, out hit, ClickCastDistance))
			{
				thing = hit.collider.gameObject;

				CheckMove(thing, hit);
				CheckChaseUnit(thing);
			}
		}
	}

	#endregion Engine Hooks

	#region Methods

	private void CheckMove(GameObject thing, RaycastHit hit)
	{
		if(thing.tag != "Terrain")
			return;

		Vector3 point = hit.point;
		_selected.SendMessage("NavigateTo", point, SendMessageOptions.DontRequireReceiver);
	}

	private void CheckChaseUnit(GameObject thing)
	{
		if(thing.tag != "Unit")
			return;

		_selected.SendMessage("ChaseUnit", thing, SendMessageOptions.DontRequireReceiver);
	}

	#endregion Methods
}
