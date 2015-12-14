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
    private MatchController Match
    {
        get
        {
            if (_match == null)
                _match = MatchController.Instance;

            return _match;
        }
    }

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

    #endregion Hooks

    #region Methods

    public void UseAbility(int abilityIndex)
    {
        PlayButtonSound();
        Ability ability = _unitSpawnAbilities[abilityIndex];

        Match.UseUnitSpawnAbility(ability);
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

            if (!string.IsNullOrEmpty(unitAbility.IconPath))
            {
                Sprite sprite = Resources.Load<Sprite>(unitAbility.IconPath);
                Image image = currentButton.transform.Find("Icon").GetComponent<Image>();

                image.overrideSprite = sprite;
            }
        }
    }

    #endregion Methods
}
