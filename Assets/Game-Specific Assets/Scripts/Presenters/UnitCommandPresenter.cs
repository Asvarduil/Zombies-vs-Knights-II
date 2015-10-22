using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCommandPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Image Background;
    public List<Button> AbilityButtons;

    private List<Ability> _unitSpawnAbilities;

    private PlayerManager _player;
    private PlayerManager Player
    {
        get
        {
            if (_player == null)
                _player = PlayerManager.Instance;

            return _player;
        }
    }

    private MapController _map;
    private MapController Map
    {
        get
        {
            if (_map == null)
                _map = MapController.Instance;

            return _map;
        }
    }

    private MatchController _match;
    private GameEventController _gameEvent;

    private GameUIMasterController _controller;
    private GameUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = GameUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _player = PlayerManager.Instance;

        _controller = GameUIMasterController.Instance;
        _map = MapController.Instance;
        _match = MatchController.Instance;
        _gameEvent = GameEventController.Instance;

        base.Start();
    }

    #endregion Hooks

    #region Methods

    public void UseAbility(int abilityIndex)
    {
        PlayButtonSound();
        Ability ability = _unitSpawnAbilities[abilityIndex];

        if (! _match.IsPurchaseSuccessful(_player.Faction, ability))
        {
            DebugMessage("Cannot afford unit...");
            Controller.PresentTooltip("Cannot afford unit...");
            return;
        }

        FormattedDebugMessage(LogLevel.Info, 
            "Presenter - using Create Unit Ability #{0} for faction {1}", 
            abilityIndex, 
            _player.Faction);

        _gameEvent.RunGameEventGroup(ability.GameEvents);
    }

    public void PresentTooltip(int abilityIndex)
    {
        string description = _unitSpawnAbilities[abilityIndex].Description;
        Controller.PresentTooltip(description);
    }

    public void HideTooltip()
    {
        Controller.HideTooltip();
    }

    public void PresentCommands()
    {
        PresentGUI(true);

        List<Ability> factionUnitSpawnAbilities = Map.GetUnitSpawnAbilities(Player.Faction);
        _unitSpawnAbilities = new List<Ability>();
        for (int i = 0; i < AbilityButtons.Count; i++)
        {
            Button currentButton = AbilityButtons[i];

            if (i > factionUnitSpawnAbilities.Count - 1)
            {
                ActivateButton(currentButton, false);
                continue;
            }

            ActivateButton(currentButton, true);
            Ability unitAbility = factionUnitSpawnAbilities[i];
            _unitSpawnAbilities.Add(unitAbility);

            // TODO: Update the button icon...
            if (!string.IsNullOrEmpty(unitAbility.IconPath))
            {
                currentButton.image = Resources.Load<Image>(unitAbility.IconPath);
            }
        }
    }

    #endregion Methods
}
