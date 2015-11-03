using System;

public class AnalyticsRepository : JsonBlobLoaderBase<AnalyticsEventModel>
{
    #region Variables / Properties

    private Guid _guid;
    private Guid GUID
    {
        get
        {
            if (_guid == null)
                _guid = Guid.NewGuid();

            return _guid;
        }
    }

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
        var model = new AnalyticsEventModel
        {
            SessionID = GUID.ToString(),
            EventType = eventType,
            EventDescription = eventDescription
        };

        // TODO: Save model.
    }

    #endregion Methods
}

