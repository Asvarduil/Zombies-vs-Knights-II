using UnityEngine;
using System;

public class ReticulePresenter : DebuggableBehavior
{
	#region Variables / Properties

	public string ChildName = "Reticule";
	public string ColorProperty = "_TintColor";
	public Color AlliedColor;
	public Color NeutralColor;
	public Color EnemyColor;

	private GameObject _reticule;
	private UnitFaction _unitFaction;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Start()
	{
		_unitFaction = GetComponent<UnitFaction>();

		_reticule = transform.FindChild(ChildName).gameObject;
		if(_reticule == null)
			DebugMessage("Could not find reticule game object '" + ChildName + "'.", LogLevel.LogicError);
	}

	public void SelectMe(Teams selectorTeam)
	{
		// TODO: Make this distingish between ally and enemy...
		DebugMessage("Unit " + gameObject.name + " has been selected...");
		PresentReticule(selectorTeam);
	}

	public void DeselectMe()
	{
		DebugMessage("Unit " + gameObject.name + " has been deselected.");
		HideReticule();
	}

	public void PresentReticule(Teams selectorTeam)
	{
		if(_reticule == null)
			return;

		DebugMessage("Showing " + selectorTeam + " reticule...");

		Color reticuleColor;
		if(selectorTeam == Teams.Spectator)
		{
			reticuleColor = NeutralColor;
		}
		else if(selectorTeam == _unitFaction.Team)
		{
			reticuleColor = AlliedColor;
		}
		else
		{
			reticuleColor = EnemyColor;
		}

		_reticule.renderer.material.SetColor(ColorProperty, reticuleColor);
	}

	public void HideReticule()
	{
		if(_reticule == null)
			return;

		DebugMessage("Hiding reticule...");

		Renderer reticuleRenderer = _reticule.renderer;
		Color reticuleColor = reticuleRenderer.material.GetColor(ColorProperty);
		reticuleColor.a = 0;

		_reticule.renderer.material.SetColor(ColorProperty, reticuleColor);
	}

	#endregion Engine Hooks
}
