using UnityEngine.UI;

public class TooltipPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Text TooltipLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ShowTooltip(string description)
    {
        TooltipLabel.text = description;
        PresentGUI(true);
    }

    public void HideTooltip()
    {
        PresentGUI(false);
    }

    #endregion Methods
}
