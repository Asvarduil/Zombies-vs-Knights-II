using System.Collections;
using UnityEngine;

public class GameUIMasterController : UIMasterControllerBase<GameUIMasterController>
{
    #region Constants

    private const string ResultsScene = "Results";

    #endregion Constants

    #region Variables / Properties

    private UnitSelectionManager _selection;
    private UnitSelectionManager Selection
    {
        get
        {
            if (_selection == null)
                _selection = UnitSelectionManager.Instance;

            return _selection;
        }
    }

    private TooltipPresenter _tooltip;
    private TooltipPresenter Tooltip
    {
        get
        {
            if (_tooltip == null)
                _tooltip = FindObjectOfType<TooltipPresenter>();

            return _tooltip;
        }
    }

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
    private EndGamePresenter EndGame
    {
        get
        {
            if (_endGame == null)
                _endGame = FindObjectOfType<EndGamePresenter>();

            return _endGame;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void PresentUnitCommands()
    {
        UnitToolbox.PresentCommands();
    }

    public void PresentTooltip(string tooltipText)
    {
        Tooltip.ShowTooltip(tooltipText);
    }

    public void HideTooltip()
    {
        Tooltip.HideTooltip();
    }

    public void SelectTargetForUnit(UnitActuator unit)
    {
        Selection.SelectTargetForUnit(unit);
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

        Selection.ClearSelection();
        Tooltip.HideTooltip();
        PlayerFaction.PresentGUI(false);
        UnitToolbox.PresentGUI(false);

        EndGame.UpdateOutcome(state);
    }

    public void ProceedToResultsScreen()
    {
        StartCoroutine(FadeAndLoadScene(ResultsScene));
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
