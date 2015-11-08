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

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToSender()
    {
        string scene = Player.Mode.ToString();
        StartCoroutine(FadeAndLoadScene(scene));
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
