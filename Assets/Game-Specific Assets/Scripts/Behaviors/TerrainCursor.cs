using UnityEngine;
using System.Collections;

public class TerrainCursor : DebuggableBehavior
{
    #region Variables / Properties

    private UnitSelectionManager _selection;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _selection = UnitSelectionManager.Instance;
    }

    public void OnMouseEnter()
    {
        _selection.SetCursor("Default");
    }

    #endregion Hooks
}
