public class CampaignSelectPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    private CampaignUIMasterController _controller;
    private CampaignUIMasterController Controller
    {
        get
        {
            if (_controller == null)
                _controller = CampaignUIMasterController.Instance;

            return _controller;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    // TODO: Pass in campaign object here...
    public void SelectCampaign(int campaignId)
    {
        PlayButtonSound();

        /*
        // TODO: Get campaign from repository by ID...
        Controller.ViewCampaign(campaign);
        */
    }

    public void ReturnToTitle()
    {
        PlayButtonSound();
        Controller.ReturnToTitle();
    }

    #endregion Hooks

    #region Methods


    #endregion Methods
}
