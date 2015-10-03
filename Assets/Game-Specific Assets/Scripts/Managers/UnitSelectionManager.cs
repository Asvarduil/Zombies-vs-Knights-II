﻿using System;
using UnityEngine;

public enum SelectionMode
{
    UnitSelect,
    UnitTarget
}

public class UnitSelectionManager : ManagerBase<UnitSelectionManager>
{
    #region Variables / Properties

    public string SelectAxis;
    public string ActionAxis;

    public Lockout SelectionLockout;
    public SelectionMode SelectionMode;
    public UnitActuator SelectedUnit;

    public string NeutralCursor;
    public string DefendCursor;
    public string AggressiveCursor;
    public string FriendlyCursor;

    private PlayerManager _player;
    private GameUIMasterController _uiController;

    private CursorRepository _cursorRepository;

    private Ray _ray;
    private RaycastHit _hit;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _player = PlayerManager.Instance;
        _uiController = GameUIMasterController.Instance;

        _cursorRepository = FindObjectOfType<CursorRepository>();

        SetCursor(NeutralCursor);
    }

    public void Update()
    {
        ProcessSelection();
        ProcessAction();
    }

    #endregion Hooks

    #region Methods

    public void SetCursor(string cursorState)
    {
        CursorModel model = _cursorRepository.GetCursorByName(cursorState);
        if (model == default(CursorModel))
        {
            DebugMessage("Could not find a cursor named " + cursorState + " in the cursor repository.");
            return;
        }

        if (model.Texture == null)
        {
            DebugMessage("No texture is associated to Cursor model " + model.Name, LogLevel.LogicError);
            return;
        }

        Cursor.SetCursor(model.Texture, model.Point, CursorMode.Auto);
    }

    public void UpdateCursor(UnitActuator unit)
    {
        string cursorModelName = NeutralCursor;

        switch (SelectionMode)
        {
            case SelectionMode.UnitSelect:
                // Do nothing; the neutral cursor is fine.
                break;

            case SelectionMode.UnitTarget:
                // If an opposing unit is selected, the only valid option is a default cursor.
                // The player cannot control opposing units.
                bool isSelectedUnitOpposingPlayer = SelectedUnit.Faction != _player.Faction;
                if (isSelectedUnitOpposingPlayer)
                {
                    cursorModelName = NeutralCursor;
                    break;
                }

                // If for some reason the unit is a null reference, go with a default cursor.
                // Log a warning, though...
                if(unit == null)
                {
                    cursorModelName = NeutralCursor;
                    DebugMessage("Attempted to change cursor for a null unit.", LogLevel.Warning);
                    break;
                }

                // If the target unit is the selected unit, the only option is for the
                // unit to defend its position.
                bool isSelf = SelectedUnit == unit;
                if (isSelf)
                {
                    cursorModelName = DefendCursor;
                    break;
                }

                // We know that A) the selected unit is on our side, and B)
                // the selected unit is not the target unit.  Therefore,
                // if the target unit isn't one of our...it's one of theirs.
                cursorModelName = unit.Faction != _player.Faction
                    ? AggressiveCursor
                    : FriendlyCursor;
                break;

            default:
                throw new InvalidOperationException("Unexpected selection mode: " + SelectionMode);
        }

        SetCursor(cursorModelName);
    }

    public void ClearSelection()
    {
        _uiController.HideUnitCommand();

        if (SelectedUnit == null)
            return;

        SelectedUnit.SetTarget(null);
        SelectedUnit = null;
    }

    private void ProcessSelection()
    {
        if (!Input.GetButton(SelectAxis) 
            || !SelectionLockout.CanAttempt())
            return;
        
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, 10000f))
        {
            GameObject target = _hit.collider.gameObject;

            switch (SelectionMode)
            {
                case SelectionMode.UnitSelect:
                case SelectionMode.UnitTarget:
                    SelectTarget(target);

                    if (SelectedUnit != null)
                        _uiController.PromptUnitCommand(SelectedUnit);
                    else
                        _uiController.HideUnitCommand();

                    SetCursor(NeutralCursor);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected selection mode: " + SelectionMode);
            }
        }

        SelectionLockout.NoteLastOccurrence();
    }

    private void ProcessAction()
    {
        if (!Input.GetButton(ActionAxis)
           || !SelectionLockout.CanAttempt())
            return;

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, 10000f))
        {
            GameObject target = _hit.collider.gameObject;

            switch (SelectionMode)
            {
                case SelectionMode.UnitSelect:
                    // Do nothing.
                    break;

                case SelectionMode.UnitTarget:
                    SelectUnitTarget(SelectedUnit, target);
                    break;

                default:
                    throw new InvalidOperationException("Unexpected selection mode: " + SelectionMode);
            }
        }

        SelectionLockout.NoteLastOccurrence();
    }

    public void SelectTarget(GameObject target)
    {
        if (target.tag == "Terrain")
        {
            DeselectCurrent();
            return;
        }

        UnitActuator hitUnit = target.GetComponent<UnitActuator>();
        if (hitUnit != null)
        {
            DeselectCurrent();
            SelectedUnit = hitUnit;
            hitUnit.SelectUnit(_player.Faction);
            SelectionMode = SelectionMode.UnitTarget;
            return;
        }
    }

    public void SelectUnitTarget(UnitActuator unit, GameObject target)
    {
        if (target.tag == "Terrain")
        {
            DeselectCurrent();
            return;
        }

        UnitActuator hitUnit = target.GetComponent<UnitActuator>();
        if (hitUnit != null)
        {
            unit.SetTarget(hitUnit);
            return;
        }
    }

    public void DeselectCurrent()
    {
        if (SelectedUnit == null)
            return;

        SelectedUnit.DeselectUnit();
        SelectedUnit = null;

        SelectionMode = SelectionMode.UnitSelect;
    }

    public void SelectTargetForUnit(UnitActuator unit)
    {

    }

    #endregion Methods
}