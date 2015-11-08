using System.Collections;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class UIMasterControllerBase<T> : ManagerBase<T>
    where T : UnityObject
{
    #region Variables / Properties

    protected Fader _fader;
    protected Fader Fader
    {
        get
        {
            if (_fader == null)
                _fader = FindObjectOfType<Fader>();

            return _fader;
        }
    }

    protected Maestro _maestro;
    protected Maestro Maestro
    {
        get
        {
            if (_maestro == null)
                _maestro = Maestro.Instance;

            return _maestro;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    protected IEnumerator FadeAndLoadScene(string sceneName)
    {
        //Maestro.FadeOut();
        Fader.FadeOut();

        while (!Fader.ScreenHidden)
        {
            yield return 0;
        }

        Application.LoadLevel(sceneName);
    }

    #endregion Methods
}
