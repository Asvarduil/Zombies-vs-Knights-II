public class SkirmishPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private SkirmishUIMasterController _controller;
    private SkirmishUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = SkirmishUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToTitle()
    {
        PlayButtonSound();
        Controller.ReturnToTitle();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
