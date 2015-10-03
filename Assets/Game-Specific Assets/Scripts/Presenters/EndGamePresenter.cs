using UnityEngine;
using UnityEngine.UI;

public class EndGamePresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public string VictoryMessage = "Victory";
    public string LossMessage = "Defeat";
    public Text OutcomeLabel;

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void UpdateOutcome(MatchState state)
    {
        if (state == MatchState.OnGoing)
            return;

        OutcomeLabel.text = state == MatchState.Victory
            ? VictoryMessage
            : LossMessage;

        PresentGUI(true);
    }

    #endregion Methods
}
