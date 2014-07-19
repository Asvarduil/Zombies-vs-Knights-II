using UnityEngine;
using System.Collections.Generic;

public class PlayerGUIManager : ManagerBase<PlayerGUIManager>
{
	#region Variables / Properties

	public bool AreCommandsVisible;

	public Vector2 MouseGUIPosition
	{
		get 
		{
			Vector2 position = Input.mousePosition;
			position.y = Screen.height - position.y;
		
			return position;
		}
	}

	public bool MouseIsInCommandGUI
	{
		get 
		{ 
			bool isInRect = _commands.InterfaceRect.Contains(MouseGUIPosition);

			DebugMessage("The mouse " + (isInRect ? "is" : "is not") + " in the command bar rectangle.");
			DebugMessage("The mouse position is: " + MouseGUIPosition);
			DebugMessage("The command bar rectangle is: " + _commands.InterfaceRect);

			return AreCommandsVisible && isInRect;
		}
	}

	private CameraClick _mainInterface;
	
	private CommandPresenter _commands;
	private PlayerUnitPresenter _unitPresenter;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_mainInterface = (CameraClick) FindObjectOfType(typeof(CameraClick));
		_commands = GetComponentInChildren<CommandPresenter>();
		_unitPresenter = GetComponentInChildren<PlayerUnitPresenter>();
	}

	#endregion Engine Hooks

	#region Event Hooks - Player Units

	public void UpdateAvailableUnits()
	{
		_unitPresenter.UpdatePopulationCounter();
		_unitPresenter.UpdateUnitPortraits();
	}

	#endregion Event Hooks - Player Units

	#region Event Hooks - Commands

	public void ShowCommands(List<UnitCommand> commands)
	{
		AreCommandsVisible = true;

		_commands.UpdateCommandBar(commands);
		_commands.SetVisibility(true);
	}

	public void HideCommands()
	{
		AreCommandsVisible = false;
		_commands.SetVisibility(false);
	}

	public void IssueCommand(int commandId)
	{
		_mainInterface.Selected.SendMessage("IssueCommand", commandId, SendMessageOptions.DontRequireReceiver);
	}

	#endregion Event Hooks - Commands
}
