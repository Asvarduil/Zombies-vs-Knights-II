using UnityEngine;
using System.Collections;

public class LoadingPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilButton CancelButton;

	private TitleController _controller;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start ()
	{
		base.Start();

		_controller = TitleController.Instance;
	}

	#endregion Engine Hooks

	#region Hooks

	public override void SetVisibility (bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		CancelButton.TargetTint.a = opacity;
	}

	public override void DrawMe ()
	{
		if(CancelButton.IsClicked())
		{
			_maestro.PlayOneShot(BackButtonSound);
			_controller.GoBackOneScreen();
			return;
		}
	}

	public override void Tween ()
	{
		CancelButton.Tween();
	}

	#endregion Hooks
}
