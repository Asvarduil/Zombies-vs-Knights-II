public class CursorRepository : JsonBlobLoaderBase<CursorModel>
{
    #region Variables / Properties

    private static CursorRepository _instance;
    public static CursorRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CursorRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Variables / Properties

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new CursorMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public CursorModel GetCursorByName(string cursorName)
    {
        var model = Contents.FindItemByName(cursorName);
        return model;
    }

    #endregion Methods
}
