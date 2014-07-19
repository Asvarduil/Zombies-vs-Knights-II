using UnityEngine;
using System;

public class TitleController : ManagerBase<TitleController>
{
	#region Variables / Properties

	private bool _isPlayingOnline;

	private MapPresenter _maps;
	private TitlePresenter _title;
	private FactionPresenter _faction;
	private LoadingPresenter _loading;
	private TutorialPresenter _tutorial;
	private SettingsPresenter _settings;

	private TeamManager _teams;
	private Matchmaker _matchmaker;

	private Action _backAction;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_maps = GetComponentInChildren<MapPresenter>();
		_title = GetComponentInChildren<TitlePresenter>();
		_faction = GetComponentInChildren<FactionPresenter>();
		_loading = GetComponentInChildren<LoadingPresenter>();
		_tutorial = GetComponentInChildren<TutorialPresenter>();
		_settings = GetComponentInChildren<SettingsPresenter>();

		_teams = TeamManager.Instance;
		_matchmaker = GetComponent<Matchmaker>();
	}

	#endregion Engine Hooks

	#region Methods

	public void GoBackOneScreen()
	{
		if(_backAction == null)
			return;

		_backAction();
	}

	public void PresentSettings()
	{
		_settings.SetVisibility(true);
		_backAction = PresentTitle;

		_maps.SetVisibility(false);
		_title.SetVisibility(false);
		_faction.SetVisibility(false);
		_loading.SetVisibility(false);
		_tutorial.SetVisibility(false);
	}

	public void PresentTitle()
	{
		_title.SetVisibility(true);
		_backAction = null;

		_maps.SetVisibility(false);
		_faction.SetVisibility(false);
		_loading.SetVisibility(false);
		_tutorial.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void PresentTutorial()
	{
		_isPlayingOnline = false;
		_backAction = PresentTitle;

		_tutorial.SetVisibility(true);

		_maps.SetVisibility(false);
		_title.SetVisibility(false);
		_faction.SetVisibility(false);
		_loading.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void PresentMultiplayer()
	{
		_isPlayingOnline = true;
		_backAction = PresentTitle;

		_faction.SetVisibility(true);

		_maps.SetVisibility(false);
		_title.SetVisibility(false);
		_loading.SetVisibility(false);
		_tutorial.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void PresentPractice()
	{
		_isPlayingOnline = false;
		_backAction = PresentTutorial;

		_faction.SetVisibility(true);

		_maps.SetVisibility(false);
		_title.SetVisibility(false);
		_loading.SetVisibility(false);
		_tutorial.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void PresentMaps(Teams playerTeam)
	{
		_teams.PlayerTeam = playerTeam;

		if(_isPlayingOnline)
			_backAction = PresentMultiplayer;
		else
			_backAction = PresentPractice;

		_maps.SetVisibility(true);

		_title.SetVisibility(false);
		_loading.SetVisibility(false);
		_faction.SetVisibility(false);
		_tutorial.SetVisibility(false);
		_settings.SetVisibility(false);
	}

	public void PresentLoadingScreen(string mapName)
	{
		if(_isPlayingOnline)
		{
			_loading.SetVisibility(true);
			_backAction = () => {
				_matchmaker.StopSeekingMatch();
				PresentMaps(_teams.PlayerTeam);
			};
			
			_maps.SetVisibility(false);
			_title.SetVisibility(false);
			_faction.SetVisibility(false);
			_tutorial.SetVisibility(false);
			_settings.SetVisibility(false);

			_matchmaker.SeekMatch();
		}
		else
		{
			// TODO: Allocate AIs and load a chosen map.
		}

		Application.LoadLevel(mapName);
	}

	#endregion Methods
}
