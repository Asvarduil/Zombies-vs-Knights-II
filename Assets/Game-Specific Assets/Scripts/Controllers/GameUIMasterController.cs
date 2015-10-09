public class GameUIMasterController : ManagerBase<GameUIMasterController>
{
    #region Variables / Properties

    private PlayerManager _player;
    private UnitSelectionManager _selection;

    private TooltipPresenter _tooltip;
    private UnitCommandPresenter _unitCommand;
    private PlayerFactionPresenter _playerFaction;
    private EndGamePresenter _endGame;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _player = PlayerManager.Instance;
        _selection = UnitSelectionManager.Instance;

        _tooltip = GetComponentInChildren<TooltipPresenter>();
        _unitCommand = GetComponentInChildren<UnitCommandPresenter>();
        _playerFaction = GetComponentInChildren<PlayerFactionPresenter>();
        _endGame = GetComponentInChildren<EndGamePresenter>();
    }

    #endregion Hooks

    #region Methods

    public void PromptUnitCommand(UnitActuator unit)
    {
        if (unit.Faction != _player.Faction)
        {
            _unitCommand.HidePrompt();
            return;
        }

        _unitCommand.PresentCommands(unit);
    }

    public void HideUnitCommand()
    {
        _unitCommand.HidePrompt();
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
