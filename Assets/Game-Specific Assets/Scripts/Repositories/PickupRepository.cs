public class PickupRepository : JsonBlobLoaderBase<PickupModel>
{
    #region Variables / Properties

    private static PickupRepository _instance;
    public static PickupRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PickupRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new PickupMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public PickupModel GetPickupByName(string pickupName)
    {
        PickupModel result = Contents.FindItemByName(pickupName);
        if (result == default(PickupModel))
            result = null;

        FormattedDebugMessage(LogLevel.Warn,
            "For pickup name {0} found {1} entry in the repository.",
            pickupName,
            (result == null ? "no" : "an"));

        return result;
    }

    #endregion Methods
}
