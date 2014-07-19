using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Matchmaker : DebuggableBehavior
{
	#region Variables / Properties

	public bool SeekingGame = false;

	public string ClientIdentifier;
	public string GameName = "ZvK2";
	public string MultiplayerLevel;

	public bool FoundGame { get; private set; }

	public string GameTypeString
	{
		get 
		{ 
			return _teams.PlayerTeam 
			       + "|" + MultiplayerLevel
				   + "|" + ClientIdentifier;
		}
	}

	private TeamManager _teams;
	private ConnectionManager _connection;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_teams = TeamManager.Instance;
		_connection = ConnectionManager.Instance;
	}

	public void Update()
	{
		if(! SeekingGame)
			return;

		StartCoroutine(_connection.FindHosts(GameName));

		List<HostData> acceptableHosts = FilterHosts();
		if(acceptableHosts.Count > 0)
		{
			FoundGame = true;
			_connection.Connect(acceptableHosts[0]);
		}
	}

	#endregion Engine Hooks

	#region Methods

	public void SeekMatch()
	{
		SeekingGame = true;
	}

	public void StopSeekingMatch()
	{
		SeekingGame = false;
	}

	public List<HostData> FilterHosts()
	{
		List<HostData> hosts = _connection.HostData;

		// First, filter out games that do not match the
		// player's specified play conditions.

		// Next, filter out games that are full.

		return hosts;
	}

	#endregion Methods
}
