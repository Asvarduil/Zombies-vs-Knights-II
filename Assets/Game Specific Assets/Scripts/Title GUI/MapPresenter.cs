using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayableMap
{
	#region Variables / Properties

	public string SceneName;
	public AsvarduilButton MapSelectButton;

	#endregion Variables / Properties
}

public class MapPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilButton BackButton;
	public List<PlayableMap> MapData;

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
		for(int i = 0; i < MapData.Count; i++)
		{
			MapData[i].MapSelectButton.TargetTint.a = opacity;
		}
	}

	public override void DrawMe ()
	{
		if(BackButton.IsClicked())
		{
			_maestro.PlayOneShot(BackButtonSound);
			_controller.GoBackOneScreen();
			return;
		}

		for(int i = 0; i < MapData.Count; i++)
		{
			if(MapData[i].MapSelectButton.IsClicked())
			{
				_maestro.PlayOneShot(ButtonSound);
				_controller.PresentLoadingScreen(MapData[i].SceneName);
				return;
			}
		}
	}

	public override void Tween ()
	{
		BackButton.Tween();

		for(int i = 0; i < MapData.Count; i++)
		{
			MapData[i].MapSelectButton.Tween();
		}
	}

	#endregion Hooks
}
