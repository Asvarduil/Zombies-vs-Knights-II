using UnityEngine;
using System.Collections;

public class FactionPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilButton KnightButton;
	public AsvarduilButton ZombieButton;
	public AsvarduilButton SpectatorButton;
	public AsvarduilButton BackButton;

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

		BackButton.TargetTint.a = opacity;
		KnightButton.TargetTint.a = opacity;
		ZombieButton.TargetTint.a = opacity;
		SpectatorButton.TargetTint.a = opacity;
	}

	public override void DrawMe ()
	{
		if(BackButton.IsClicked())
		{
			_maestro.PlayOneShot(BackButtonSound);
			_controller.GoBackOneScreen();
			return;
		}

		if(KnightButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentMaps(Teams.Knights);
			return;
		}

		if(ZombieButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentMaps(Teams.Zombies);
			return;
		}

		if(SpectatorButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentMaps(Teams.Spectator);
			return;
		}
	}

	public override void Tween()
	{
		BackButton.Tween();
		KnightButton.Tween();
		ZombieButton.Tween();
		SpectatorButton.Tween();
	}

	#endregion Hooks
}
