using System.Collections.Generic;

public class CampaignRepository : JsonBlobLoaderBase<CampaignModel>
{
    #region Variables / Properties

    private static CampaignRepository _instance;
    public static CampaignRepository Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CampaignRepository>();

            return _instance;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Awake()
    {
        _mapper = new CampaignMap();
        MapJsonBlob();
    }

    #endregion Hooks

    #region Methods

    public List<CampaignModel> GetAllCampaigns()
    {
        return Contents;
    }

    #endregion Methods
}
