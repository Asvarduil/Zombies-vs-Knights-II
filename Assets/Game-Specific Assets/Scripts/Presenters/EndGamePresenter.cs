using UnityEngine;
using UnityEngine.UI;

public class EndGamePresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public string VictoryMessage = "Victory";
    public string LossMessage = "Defeat";
    public Text OutcomeLabel;

    private GameUIMasterController _controller;
    private GameUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = GameUIMasterController.Instance;

            return _controller;
        }
    }

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

    public void ProceedToResults()
    {
        PlayButtonSound();
        Controller.ProceedToResultsScreen();
    }

    #endregion Methods
}
