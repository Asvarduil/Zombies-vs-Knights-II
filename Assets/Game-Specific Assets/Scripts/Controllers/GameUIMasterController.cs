public class GameUIMasterController : ManagerBase<GameUIMasterController>
{
    #region Variables / Properties

    private UnitSelectionManager _selection;

    private TooltipPresenter _tooltip;

    private PlayerFactionPresenter _playerFaction;
    public PlayerFactionPresenter PlayerFaction
    {
        get
        {
            if (_playerFaction == null)
                _playerFaction = FindObjectOfType<PlayerFactionPresenter>();

            return _playerFaction;
        }
    }

    private UnitCommandPresenter _unitToolbox;
    private UnitCommandPresenter UnitToolbox
    {
        get
        {
            if (_unitToolbox == null)
                _unitToolbox = FindObjectOfType<UnitCommandPresenter>();

            return _unitToolbox;
        }
    }

    private EndGamePresenter _endGame;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _selection = UnitSelectionManager.Instance;

        _tooltip = GetComponentInChildren<TooltipPresenter>();
        _unitToolbox = GetComponentInChildren<UnitCommandPresenter>();
        _playerFaction = GetComponentInChildren<PlayerFactionPresenter>();
        _endGame = GetComponentInChildren<EndGamePresenter>();
    }

    #endregion Hooks

    #region Methods

    public void PresentUnitCommands()
    {
        UnitToolbox.PresentCommands();
    }

    public void PresentTooltip(string tooltipText)
    {
        _tooltip.ShowTooltip(tooltipText);
    }

    public void HideTooltip()
    {
        _tooltip.HideTooltip();
    }

    public void SelectTargetForUnit(UnitActuator unit)
    {
        _selection.SelectTargetForUnit(unit);
    }

    public void UpdateKeyStructureHP(int HP, int maxHP)
    {
        PlayerFaction.UpdateKeyUnitHP(HP, maxHP);
    }

    public void UpdateResourceCount(int resources, int maxResources)
    {
        PlayerFaction.UpdateResource(resources, maxResources);
    }

    public void ShowMatchOutcome(MatchState state)
    {
        if (state == MatchState.OnGoing)
            return;

        _selection.ClearSelection();
        _endGame.UpdateOutcome(state);
    }

    #endregion Methods
}
