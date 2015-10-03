public class BuffRepository : JsonBlobLoaderBase<Buff>
{
    #region Variables / Properties

    private static BuffRepository _instance;
    public static BuffRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BuffRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new BuffMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public Buff GetBuffByName(string buffName)
    {
        Buff result = Contents.FindItemByName(buffName);
        if (result == default(Buff))
            result = null;

        return result;
    }

    #endregion Methods
}
