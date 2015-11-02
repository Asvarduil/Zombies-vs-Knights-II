public class AnalyticsRepository : JsonBlobLoaderBase<AnalyticsEventModel>
{
    #region Variables / Properties

    private static AnalyticsRepository _instance;
    public static AnalyticsRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AnalyticsRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _mapper = new AnalyticsEventMap();
    }

    #endregion Hooks

    #region Methods

    public void LogEvent(string eventType, string eventDescription)
    {

    }

    #endregion Methods
}

