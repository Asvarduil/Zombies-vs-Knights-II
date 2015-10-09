using System.Collections.Generic;
using UnityEngine.UI;

public class UnitCommandPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public Image Background;
    public Text UnitNameLabel;
    public List<Button> AbilityButtons;

    private UnitActuator _unit;
    private List<Ability> _unitAbilities;

    private MatchController _match;
    private GameUIMasterController _controller;

    #endregion Variables / Properties

    #region Hooks

    public override void Start()
    {
        _controller = GameUIMasterController.Instance;
        _match = MatchController.Instance;

        base.Start();
    }

    #endregion Hooks

    #region Methods

    public void UseAbility(int abilityIndex)
    {
        DebugMessage("Presenter - using Unit Ability #" + abilityIndex);
        PlayButtonSound();

        // TODO: Use unit's ability.
        _match.UseSelectedUnitAbility(abilityIndex);
    }

    public void PresentTooltip(int abilityIndex)
    {
        if (_unit == null)
            return;

        if (_unit.Abilities.IsNullOrEmpty())
            return;

        string description = _unit.Abilities[abilityIndex].Description;
        _controller.PresentTooltip(description);
    }

    public void HideTooltip()
    {
        _controller.HideTooltip();
    }

    public void PresentCommands(UnitActuator unit)
    {
        _unit = unit;

        UnitNameLabel.text = _unit.Name;

        PresentGUI(true);
        _unitAbilities = new List<Ability>();
        for (int i = 0; i < AbilityButtons.Count; i++)
        {
            Button currentButton = AbilityButtons[i];

            if (i > unit.Abilities.Count - 1)
            {
                ActivateButton(currentButton, false);
                continue;
            }

            ActivateButton(currentButton, true);
            Ability unitAbility = unit.Abilities[i];
            _unitAbilities.Add(unitAbility);

            // TODO: Update the button...
        }
    }

    public void HidePrompt()
    {
        _unit = null;
        PresentGUI(false);
    }

    #endregion Methods
}
