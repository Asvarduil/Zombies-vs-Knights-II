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
