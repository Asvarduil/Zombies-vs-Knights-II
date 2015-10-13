﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCommandPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Image Background;
    public List<Button> AbilityButtons;

    private List<Ability> _unitSpawnAbilities;

    private PlayerManager _player;

    private MapController _map;
    private GameEventController _gameEvent;
    private GameUIMasterController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _player = PlayerManager.Instance;

        _controller = GameUIMasterController.Instance;
        _map = MapController.Instance;
        _gameEvent = GameEventController.Instance;

        base.Start();

        PresentCommands();
    }

    #endregion Hooks

    #region Methods

    public void UseAbility(int abilityIndex)
    {
        FormattedDebugMessage(LogLevel.Information, 
            "Presenter - using Create Unit Ability #{0} for faction {1}", 
            abilityIndex, 
            _player.Faction);

        PlayButtonSound();
        Ability ability = _unitSpawnAbilities[abilityIndex];
        _gameEvent.RunGameEventGroup(ability.GameEvents);
    }

    public void PresentTooltip(int abilityIndex)
    {
        string description = _unitSpawnAbilities[abilityIndex].Description;
        _controller.PresentTooltip(description);
    }

    public void HideTooltip()
    {
        _controller.HideTooltip();
    }

    public void PresentCommands()
    {
        PresentGUI(true);

        List<Ability> factionUnitSpawnAbilities = _map.GetUnitSpawnAbilities(_player.Faction);
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