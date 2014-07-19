using UnityEngine;
using System.Collections;

public class StaticElementsPresenter : PresenterBase
{
	#region Variables / Properties
	
	public AsvarduilBox MinimapMask;

	#endregion Variables / Properties

	#region Methods

	#endregion Methods

	#region Hooks

	public override void SetVisibility(bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		MinimapMask.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		MinimapMask.DrawMe();
	}

	public override void Tween()
	{
		MinimapMask.Tween();
	}

	#endregion Hooks
}
