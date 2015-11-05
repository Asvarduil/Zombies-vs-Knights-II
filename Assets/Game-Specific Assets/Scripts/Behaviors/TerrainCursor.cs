using UnityEngine;
using System.Collections;

public class TerrainCursor : DebuggableBehavior
{
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

    #endregion Variables / Properties

    #region Hooks

    public void OnMouseEnter()
    {
        Selection.SetCursor("Default");
    }

    #endregion Hooks
}
