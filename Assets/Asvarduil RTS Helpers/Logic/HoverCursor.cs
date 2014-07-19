using UnityEngine;
using System;

[Serializable]
public class CursorRelationship
{
	public Texture2D CursorImage;
	public Vector2 CursorHotspot;
}

public class HoverCursor : DebuggableBehavior
{
	#region Variables / Properties

	public CursorRelationship AllyCursor;
	public CursorRelationship NeutralCursor;
	public CursorRelationship EnemyCursor;

	private UnitFaction _faction;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_faction = GetComponent<UnitFaction>();
	}

	public void OnMouseEnter()
	{
		CursorRelationship relationship = DetermineFactionCursorRelationship();
		Cursor.SetCursor(relationship.CursorImage, relationship.CursorHotspot, CursorMode.Auto);
	}

	#endregion Engine Hooks

	#region Methods

	private CursorRelationship DetermineFactionCursorRelationship()
	{
		Teams PlayerTeam = TeamManager.Instance.PlayerTeam;
		if(_faction == null
		   || PlayerTeam == Teams.Spectator)
		{
			DebugMessage("This unit is neutral.");
			return NeutralCursor;
		}

		if(PlayerTeam == _faction.Team)
		{
			DebugMessage("This unit is one of our allies.");
			return AllyCursor;
		}

		DebugMessage("This unit is an enemy!");
		return EnemyCursor;
	}

	#endregion Methods
}
