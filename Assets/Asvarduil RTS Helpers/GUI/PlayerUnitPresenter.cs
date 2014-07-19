using UnityEngine;
using System.Collections;

public class PlayerUnitPresenter : PresenterBase
{
	#region Variables / Properties

	public AsvarduilImage MaxPopulationIcon;
	public AsvarduilLabel PopulationSaturationLabel;
	public AsvarduilImageButton PrototypeUnitPortrait;
	public AsvarduilImageButton[] UnitPortraits;

	#endregion Variables / Properties

	#region Methods

	public void UpdateUnitPortraits()
	{
		// TODO: Update the unit images
	}

	public void UpdatePopulationCounter()
	{
		// TODO: Update the population counter.
	}

	#endregion Methods

	#region Hooks

	public override void SetVisibility (bool isVisible)
	{
		float opacity = DetermineOpacity(isVisible);

		MaxPopulationIcon.TargetTint.a = opacity;
		PopulationSaturationLabel.TargetTint.a = opacity;
	}

	public override void DrawMe()
	{
		MaxPopulationIcon.DrawMe();
		PopulationSaturationLabel.DrawMe();

		for(int i = 0; i < UnitPortraits.Length; i++)
		{
			if(UnitPortraits[i].IsClicked())
			{
				// TODO: Send a message to select/move
				//       the camera to the selected unit
			}
		}
	}

	public override void Tween()
	{
		MaxPopulationIcon.Tween();
		PopulationSaturationLabel.Tween();

		for(int i = 0; i < UnitPortraits.Length; i++)
		{
			UnitPortraits[i].Tween();
		}
	}

	#endregion Hooks
}
