using UnityEngine;
using System.Collections;

public class SettingsPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilButton BackButton;

	#endregion Variables / Properties

	#region Engine Hooks

	public override void Start()
	{
		base.Start();
	}

	#endregion Engine Hooks

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		BackButton.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		if(BackButton.IsClicked())
		{
			_maestro.PlayOneShot(BackButtonSound);
			return;
		}
	}

	public override void Tween()
	{
		BackButton.Tween();
	}

	#endregion Hooks
}
