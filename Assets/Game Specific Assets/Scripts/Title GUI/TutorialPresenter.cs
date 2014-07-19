using UnityEngine;
using System.Collections;

public class TutorialPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilButton BackButton;
	public AsvarduilButton BasicTutorialButton;
	public AsvarduilButton IntermediateTutorialButton;
	public AsvarduilButton AdvancedTutorialButton;
	public AsvarduilButton PracticeMatchButton;

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

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		BackButton.TargetTint.a = opacity;
		BasicTutorialButton.TargetTint.a = opacity;
		IntermediateTutorialButton.TargetTint.a = opacity;
		AdvancedTutorialButton.TargetTint.a = opacity;
		PracticeMatchButton.TargetTint.a = opacity;
	}

	public override void DrawMe ()
	{
		if(BackButton.IsClicked())
		{
			_maestro.PlayOneShot(BackButtonSound);
			_controller.GoBackOneScreen();
			return;
		}

		if(BasicTutorialButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			// TODO: Implement method that loads a single-player basic tutorial...
			return;
		}

		if(IntermediateTutorialButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			// TODO: Implement GUI for unit-specific tutorials...
			return;
		}

		if(AdvancedTutorialButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			// TODO: Implement GUI for advanced technique tutorials...
			return;
		}

		if(PracticeMatchButton.IsClicked())
		{
			_maestro.PlayOneShot(ButtonSound);
			_controller.PresentPractice();
			return;
		}
	}

	public override void Tween ()
	{
		BackButton.Tween();
		BasicTutorialButton.Tween();
		IntermediateTutorialButton.Tween();
		AdvancedTutorialButton.Tween();
		PracticeMatchButton.Tween();
	}

	#endregion Hooks
}
