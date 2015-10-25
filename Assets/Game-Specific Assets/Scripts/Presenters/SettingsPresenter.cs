using UnityEngine;
using System.Collections;

public class SettingsPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private TitleUIMasterController _controller;
    private TitleUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = TitleUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ReturnToTitle()
    {
        PlayButtonSound();
        Controller.PresentTitle();
    }

    #endregion Methods
}
