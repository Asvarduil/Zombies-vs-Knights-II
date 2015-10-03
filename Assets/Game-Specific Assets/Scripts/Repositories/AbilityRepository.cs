public class AbilityRepository : JsonBlobLoaderBase<Ability>
{
    #region Variables / Properties

    private static AbilityRepository _instance;
    public static AbilityRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AbilityRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new AbilityMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public Ability GetAbilityByName(string abilityName)
    {
        Ability result = Contents.FindItemByName(abilityName);
        if (result == default(Ability))
            result = null;

        return result;
    }

    #endregion Methods
}
