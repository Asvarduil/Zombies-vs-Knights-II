
public class TitleCommandPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private TitleUIMasterController _controller;
    private TitleUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = TitleUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ShowSettings()
    {
        PlayButtonSound();

        Controller.SwitchToSettings();
    }

    public void EndGame()
    {
        PlayButtonSound();

        Controller.EndGame();
    }

    #endregion Methods
}
