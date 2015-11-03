using UnityEngine;
using System;
using System.Collections;

using CursorDriver = UnityEngine.Cursor;

public class InitializationScenePass : MonoBehaviour
{
    #region Variables / Properties

    public string NextSceneName;

    private bool _repositoriesLoaded = false;

    private AnalyticsRepository _analytics;
    private AnalyticsRepository Analytics
    {
        get
        {
            if (_analytics == null)
                _analytics = AnalyticsRepository.Instance;

            return _analytics;
        }
    }

    private UnitRepository _units;
    private UnitRepository Units
    {
        get
        {
            if (_units == null)
                _units = UnitRepository.Instance;

            return _units;
        }
    }

    private AbilityRepository _abilities;
    private AbilityRepository Abilities
    {
        get
        {
            if (_abilities == null)
                _abilities = AbilityRepository.Instance;

            return _abilities;
        }
    }

    private CursorRepository _cursor;
    private CursorRepository Cursor
    {
        get
        {
            if (_cursor == null)
                _cursor = CursorRepository.Instance;

            return _cursor;
        }
    }

    #endregion Variables / Properties

    #region Engine Hooks

    public void Start()
    {
        _units = FindObjectOfType<UnitRepository>();
        _abilities = FindObjectOfType<AbilityRepository>();

        StartCoroutine(WaitForRepositoriesToLoad());
    }

    #endregion Engine Hooks

    #region Methods

    public IEnumerator WaitForRepositoriesToLoad()
    {
        while(! _repositoriesLoaded)
        {
            _repositoriesLoaded = Units.HasLoaded 
                                  && Abilities.HasLoaded 
                                  && Cursor.HasLoaded;

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        CursorModel cursor = Cursor.GetCursorByName("Default");
        CursorDriver.SetCursor(cursor.Texture, cursor.Point, CursorMode.Auto);

        Analytics.LogEvent("GameStart", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"));

        Application.LoadLevel(NextSceneName);
    }

    #endregion Methods
}
