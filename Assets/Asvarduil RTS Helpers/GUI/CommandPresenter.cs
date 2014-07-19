using UnityEngine;
using System.Collections.Generic;

public class CommandPresenter : PresenterBase
{
	#region Variables / Properties

	public Rect InterfaceRect
	{
		get { return Background.Rectangle; }
	}

	public AsvarduilBox Background;
	public AsvarduilImageButton[] Commands;

	private PlayerGUIManager _manager;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();

		_manager = PlayerGUIManager.Instance;
	}

	#endregion Engine Hooks

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		Background.TargetTint.a = opacity;
		for(int i = 0; i < Commands.Length; i++)
		{
			Commands[i].TargetTint.a = opacity;
		}
	}

	public override void DrawMe()
	{
		Background.DrawMe();

		for(int i = 0; i < Commands.Length; i++)
		{
			if(Commands[i].IsClicked())
			{
				_maestro.PlayOneShot(ButtonSound);
				_manager.IssueCommand(i);
			}
		}
	}

	public override void Tween()
	{
		Background.Tween();

		for(int i = 0; i < Commands.Length; i++)
		{
			Commands[i].Tween();
		}
	}
	
	#endregion Hooks

	#region Methods

	public void UpdateCommandBar(List<UnitCommand> commands)
	{
		// TODO: Update the tooltip text and images of the command bar items.
	}

	#endregion Methods
}
