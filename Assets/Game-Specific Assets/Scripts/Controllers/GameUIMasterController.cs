using System.Collections;
using UnityEngine;

public class GameUIMasterController : ManagerBase<GameUIMasterController>
{
    #region Constants

    private const string ResultsScene = "Results";

    #endregion Constants

    #region Variables / Properties

    private Fader _fader;
    private Fader Fader
    {
        get
        {
            if (_fader == null)
                _fader = FindObjectOfType<Fader>();

            return _fader;
        }
    }

    private Maestro _maestro;
    private Maestro Maestro
    {
        get
        {
            if (_maestro == null)
                _maestro = Maestro.Instance;

            return _maestro;
        }
    }

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

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        //Maestro.FadeOut();
        Fader.FadeOut();

        while (!Fader.ScreenHidden)
        {
            yield return 0;
        }

        Application.LoadLevel(sceneName);
    }

    #endregion Methods
}
