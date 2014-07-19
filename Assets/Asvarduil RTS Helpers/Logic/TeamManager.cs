using UnityEngine;
using System;

public enum Teams
{
	Spectator,
	Knights,
	Zombies
}

public class TeamManager : ManagerBase<TeamManager>
{
	#region Variables / Properties

	public Teams PlayerTeam = Teams.Spectator;

	public string PlayerTeamName
	{
		get { return PlayerTeam.ToString(); }
	}

	public bool IsSpectator
	{
		get { return PlayerTeam == Teams.Spectator; }
	}

	#endregion Variables / Properties

	#region Methods

	public void AssignTeam(string teamName)
	{
		PlayerTeam = (Teams) Enum.Parse(typeof(Teams), teamName);
	}

	#endregion Methods
}
