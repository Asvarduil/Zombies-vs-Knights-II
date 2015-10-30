﻿using System.Collections;
using UnityEngine;

public class CampaignUIMasterController : ManagerBase<CampaignUIMasterController>
{
    #region Constants

    private const string TitleScene = "Title";
    private const string MatchScene = "Match";

    #endregion Constants

    #region Variables / Properties

    private Maestro _maestro;
    private Maestro Maestro
    {
        get
        {
            if (_maestro == null)
                _maestro = Maestro.Instance;

            return _maestro;
        }
    }

    private Fader _fader;
    private Fader Fader
    {
        get
        {
            if (_fader == null)
                _fader = FindObjectOfType<Fader>();

            return _fader;
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
        CampaignSelect.PresentGUI(false);
        CampaignStageSelect.LoadCampaign(model);
    }

    public void ReturnToTitle()
    {
        StartCoroutine(FadeAndLoadScene(TitleScene));
    }

    public void OpenStage(CampaignStageModel model)
    {
        // TODO: Load information into the Ambassador.
        StartCoroutine(FadeAndLoadScene(MatchScene));
    }

    #endregion Hooks

    #region Methods

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        //Maestro.FadeOut();
        Fader.FadeOut();

        while (!Fader.ScreenHidden)
        {
            yield return 0;
        }

        Application.LoadLevel(sceneName);
    }

    #endregion Methods
}