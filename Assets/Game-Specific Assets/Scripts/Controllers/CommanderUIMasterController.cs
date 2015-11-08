public class CommanderUIMasterController : UIMasterControllerBase<CommanderUIMasterController>
{
    #region Constants

    private const string TitleScene = "Title";

    #endregion Constants

    #region Variables / Properties

    private CommanderSelectPresenter _commanderSelect;
    private CommanderSelectPresenter CommanderSelect
    {
        get
        {
            if (_commanderSelect == null)
                _commanderSelect = FindObjectOfType<CommanderSelectPresenter>();

            return _commanderSelect;
        }
    }

    private CommanderPresenter _commander;
    private CommanderPresenter Commander
    {
        get
        {
            if (_commander == null)
                _commander = FindObjectOfType<CommanderPresenter>();

            return _commander;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToTitle()
    {
        StartCoroutine(FadeAndLoadScene(TitleScene));
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
