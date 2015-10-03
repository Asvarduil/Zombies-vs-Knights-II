using UnityEngine;
using System.Collections;

public class InitializationScenePass : MonoBehaviour
{
    #region Variables / Properties

    public string NextSceneName;

    private bool _repositoriesLoaded = false;
    private UnitRepository _units;
    private AbilityRepository _abilities;

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
            _repositoriesLoaded = _units.HasLoaded && _abilities.HasLoaded;
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        Application.LoadLevel(NextSceneName);
    }

    #endregion Methods
}
