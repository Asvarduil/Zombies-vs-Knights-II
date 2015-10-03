public class UnitRepository : JsonBlobLoaderBase<UnitModel>
{
    #region Variables / Properties

    private static UnitRepository _instance;
    public static UnitRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<UnitRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties
    #region Hooks

    public void Awake()
    {
        _mapper = new UnitMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public UnitModel GetUnitByName(string name)
    {
        UnitModel result = Contents.FindItemByName(name);
        if (result == default(UnitModel))
            result = null;

        DebugMessage("For unit name " + name + " " + (result == null ? "found" : "didn't find") + " an entry in the repository.");
        return result;
    }

    #endregion Methods
}
