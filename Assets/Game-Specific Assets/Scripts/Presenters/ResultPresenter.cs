public class ResultPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private ResultUIMasterController _controller;
    private ResultUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = ResultUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToSender()
    {
        Controller.ReturnToSender();
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
