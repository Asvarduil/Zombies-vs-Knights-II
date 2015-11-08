using UnityEngine;
using System.Collections;

public class SkirmishUIMasterController : UIMasterControllerBase<SkirmishUIMasterController> {
    #region Constants

    private const string TitleScene = "Title";
    private const string MatchScene = "Match";

    #endregion Constants

    #region Variables / Properties

    private SkirmishPresenter _skirmish;
    private SkirmishPresenter Skirmish
    {
        get
        {
            if (_skirmish == null)
                _skirmish = FindObjectOfType<SkirmishPresenter>();

            return _skirmish;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void ReturnToTitle()
    {
        StartCoroutine(FadeAndLoadScene(TitleScene));
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
