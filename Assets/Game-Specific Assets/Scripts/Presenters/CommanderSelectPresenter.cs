public class CommanderSelectPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private CommanderUIMasterController _controller;
    private CommanderUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = CommanderUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToTitle()
    {
        Controller.ReturnToTitle();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
