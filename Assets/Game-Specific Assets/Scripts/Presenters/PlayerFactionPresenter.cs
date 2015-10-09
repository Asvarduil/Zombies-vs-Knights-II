using UnityEngine.UI;

public class PlayerFactionPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Image Background;
    public Text KeyUnitHPLabel;
    public Text ResourceLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void UpdateKeyUnitHP(int HP, int maxHP)
    {
        string newText = string.Format("{0}/{1}", HP, maxHP);
        KeyUnitHPLabel.text = newText;
    }

    public void UpdateResource(int resources, int maxResources)
    {
        string newText = string.Format("{0}/{1}", resources, maxResources);
        ResourceLabel.text = newText;
    }

    #endregion Methods
}
