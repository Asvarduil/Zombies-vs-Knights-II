using System.Collections;
using UnityEngine;

public class CampaignUIMasterController : UIMasterControllerBase<CampaignUIMasterController>
{
    #region Constants

    private const string TitleScene = "Title";
    private const string MatchScene = "Match";

    #endregion Constants

    #region Variables / Properties

    private PlayerManager _player;
    private PlayerManager Player
    {
        get
        {
            if (_player == null)
                _player = PlayerManager.Instance;

            return _player;
        }
    }

    private CampaignSelectPresenter _campaignSelect;
    private CampaignSelectPresenter CampaignSelect
    {
        get
        {
            if (_campaignSelect == null)
                _campaignSelect = FindObjectOfType<CampaignSelectPresenter>();

            return _campaignSelect;
        }
    }

    private CampaignStageSelectPresenter _campaignStageSelect;
    private CampaignStageSelectPresenter CampaignStageSelect
    {
        get
        {
            if (_campaignStageSelect == null)
                _campaignStageSelect = FindObjectOfType<CampaignStageSelectPresenter>();

            return _campaignStageSelect;
        }
    }

    private AnalyticsRepository _analytics;
    private AnalyticsRepository Analytics
    {
        get
        {
            if (_analytics == null)
                _analytics = AnalyticsRepository.Instance;

            return _analytics;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        ViewCampaignSelect();
    }

    public void ViewCampaignSelect()
    {
        CampaignStageSelect.PresentGUI(false);
        CampaignSelect.PresentGUI(true);
    }

    public void ViewCampaign(CampaignModel model)
    {
        Player.RecordCampaignName(model.Name);
        CampaignSelect.PresentGUI(false);
        CampaignStageSelect.LoadCampaign(model);
    }

    public void ReturnToTitle()
    {
        StartCoroutine(FadeAndLoadScene(TitleScene));
    }

    public void OpenStage(CampaignStageModel model)
    {
        Analytics.LogEvent("PlayedLevel", model.Name);
        Player.Setup("Progress", model.MapName, model.PlayerFaction);
        StartCoroutine(FadeAndLoadScene(MatchScene));
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
