using UnityEngine;
using System.Collections.Generic;

public class UnitCommands : DebuggableBehavior
{
	#region Variables / Properties

	public List<UnitCommand> Commands;

	private UnitFaction _unitFaction;
	private PlayerGUIManager _guiManager;

	#endregion Variables / Properties

	#region Hooks

	public void SelectMe(Teams team)
	{
		if(team == Teams.Spectator
		   || team != _unitFaction.Team)
			return;

		DebugMessage("Showing commands for " + gameObject.name);
		_guiManager.ShowCommands(Commands);
	}

	public void DeselectMe()
	{
		DebugMessage("Hiding commands for " + gameObject.name);
		_guiManager.HideCommands();
	}

	#endregion Engine Hooks
	
	#region Engine Hooks

	public void Start()
	{
		_unitFaction = GetComponent<UnitFaction>();
		_guiManager = PlayerGUIManager.Instance;
	}

	#endregion Engine Hooks
}
