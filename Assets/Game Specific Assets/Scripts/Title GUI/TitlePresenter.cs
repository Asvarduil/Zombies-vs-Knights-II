using UnityEngine;

public class TitlePresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilImage TitlePane;
	public AsvarduilButton PracticeButton;
	public AsvarduilButton PvPButton;
	public AsvarduilButton SettingsButton;

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

		TitlePane.TargetTint.a = opacity;
		PvPButton.TargetTint.a = opacity;
		PracticeButton.TargetTint.a = opacity;
		SettingsButton.TargetTint.a = opacity;
	}

	public override void DrawMe ()
	{
		TitlePane.DrawMe();

		if(PvPButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentMultiplayer();
			return;
		}

		if(PracticeButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentTutorial();
			return;
		}

		if(SettingsButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentSettings();
			return;
		}
	}

	public override void Tween ()
	{
		TitlePane.Tween();
		PvPButton.Tween();
		PracticeButton.Tween();
		SettingsButton.Tween();
	}

	#endregion Hooks
}
