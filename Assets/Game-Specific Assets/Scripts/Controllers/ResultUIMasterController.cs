using System.Collections;
using UnityEngine;

public class ResultUIMasterController : UIMasterControllerBase<ResultUIMasterController>
{
    #region Variables / Properties

    private ResultPresenter _result;
    private ResultPresenter Result
    {
        get
        {
            if (_result == null)
                _result = FindObjectOfType<ResultPresenter>();

            return _result;
        }
    }

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

    private CursorRepository _cursor;
    private CursorRepository CursorRepository
    {
        get
        {
            if (_cursor == null)
                _cursor = CursorRepository.Instance;

            return _cursor;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        SetCursor("Default");
    }

    public void ReturnToSender()
    {
        string scene = Player.Mode.ToString();
        StartCoroutine(FadeAndLoadScene(scene));
    }

    #endregion Hooks

    #region Methods

    public void SetCursor(string cursorState)
    {
        CursorModel model = CursorRepository.GetCursorByName(cursorState);
        if (model == default(CursorModel))
        {
            DebugMessage("Could not find a cursor named " + cursorState + " in the cursor repository.");
            return;
        }

        if (model.Texture == null)
        {
            DebugMessage("No texture is associated to Cursor model " + model.Name, LogLevel.Error);
            return;
        }

        Cursor.SetCursor(model.Texture, model.Point, CursorMode.Auto);
    }

    #endregion Methods
}
