using UnityEngine;
using UnityObject = UnityEngine.Object;

public class ManagerBase<T>
    : DebuggableBehavior
    where T : UnityObject
{
    #region Variables / Properties

    private static T _instance;
    public static T Instance
    {
        get
        {
            try
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();

                return _instance;
            }
            catch (MissingReferenceException)
            {
                _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
    }

    #endregion Variables / Properties

    #region Methods

    #endregion Methods
}
