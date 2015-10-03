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

    public void UpdateKeyUnitHP(UnitActuator keyUnit)
    {
        ModifiableStat hpStat = keyUnit.Stats.FindItemByName("HP");
        string newText = string.Format("{0}/{1}", hpStat.Value, hpStat.ValueCap);
        KeyUnitHPLabel.text = newText;
    }

    public void UpdateResource()
    {
        // TODO: Figure out.
    }

    #endregion Methods
}
