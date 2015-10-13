public class GameUIMasterController : ManagerBase<GameUIMasterController>
{
    #region Variables / Properties

    private UnitSelectionManager _selection;

    private TooltipPresenter _tooltip;
    private PlayerFactionPresenter _playerFaction;
    private EndGamePresenter _endGame;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _selection = UnitSelectionManager.Instance;

        _tooltip = GetComponentInChildren<TooltipPresenter>();
        _playerFaction = GetComponentInChildren<PlayerFactionPresenter>();
        _endGame = GetComponentInChildren<EndGamePresenter>();
    }

    #endregion Hooks

    #region Methods

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
        _playerFaction.UpdateKeyUnitHP(HP, maxHP);
    }

    public void UpdateResourceCount(int resources, int maxResources)
    {
        _playerFaction.UpdateResource(resources, maxResources);
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
