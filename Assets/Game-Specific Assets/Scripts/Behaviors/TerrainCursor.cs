using UnityEngine;
using System.Collections;

public class TerrainCursor : DebuggableBehavior
{
    #region Variables / Properties

    public string CursorState = "Disabled";

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
        Selection.SetCursor(CursorState);
    }

    #endregion Hooks
}
