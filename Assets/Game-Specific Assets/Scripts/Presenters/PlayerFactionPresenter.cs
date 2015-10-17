using UnityEngine.UI;

public class PlayerFactionPresenter : UGUIPresenterBase
{
    #region Constants

    private const string DetailColor = "#999999";
    private const string HighColor = "#66FF99";
    private const string MidColor = "#CCCC33";
    private const string LowColor = "#FF6666";

    #endregion Constants

    #region Variables / Properties

    public Image Background;
    public Gauge KeyUnitHPGauge;
    public Text KeyUnitHPLabel;
    public Gauge ResourceGauge;
    public Text ResourceLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void UpdateKeyUnitHP(int HP, int maxHP)
    {
        float percent = HP / (1.0f * maxHP);
        string color = GetColorByPercentage(percent);

        string newText = string.Format("<color='{0}'>{1}</color><color='{3}'>/{2}</color>", 
            color, 
            HP, 
            maxHP, 
            DetailColor);

        KeyUnitHPLabel.text = newText;
        KeyUnitHPGauge.RecalculateGaugeSize(HP, maxHP);
    }

    public void UpdateResource(int resources, int maxResources)
    {
        float percent = resources / (1.0f * maxResources);
        string color = GetColorByPercentage(percent);

        string newText = string.Format("<color='{0}'>{1}</color><color='{3}'>/{2}</color>", 
            color, 
            resources, 
            maxResources, 
            DetailColor);

        ResourceLabel.text = newText;
        ResourceGauge.RecalculateGaugeSize(resources, maxResources);
    }

    private string GetColorByPercentage(float percentage)
    {
        string color = HighColor;

        if (percentage < 0.33f)
            color = LowColor;
        else if (percentage < 0.66f)
            color = MidColor;
        

        FormattedDebugMessage(LogLevel.Information,
            "For percentage {0} chose color {1}",
            percentage,
            color);

        return color;
    }

    #endregion Methods
}
