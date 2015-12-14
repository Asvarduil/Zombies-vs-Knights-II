public class AIRepository : JsonBlobLoaderBase<AIModel>
{
    #region Variables / Properties

    private static AIRepository _instance;
    public static AIRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AIRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new AIMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public AIModel GetAIScriptByName(string name)
    {
        AIModel result = Contents.FindItemByName(name);
        if (result == default(AIModel))
            return null;

        return result;
    }

    #endregion Methods
}
