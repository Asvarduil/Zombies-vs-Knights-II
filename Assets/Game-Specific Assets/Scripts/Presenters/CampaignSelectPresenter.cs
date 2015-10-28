using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignSelectPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public int OptionMargin;
    public GameObject CampaignOptionPrefab;
    public GameObject OptionBindPoint;

    private List<CampaignModel> _campaigns;

    private CampaignRepository _repository;
    private CampaignRepository Repository
    {
        get
        {
            if (_repository == null)
                _repository = CampaignRepository.Instance;

            return _repository;
        }
    }

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

    public override void Start()
    {
        _campaigns = Repository.GetAllCampaigns();
        SetUpCampaigns();

        base.Start();
    }

    // TODO: Pass in campaign object here...
    public void SelectCampaign(CampaignModel model)
    {
        PlayButtonSound();

        FormattedDebugMessage(LogLevel.Info,
            "Campaign {0} was selected.",
            model.Name);

        Controller.ViewCampaign(model);
    }

    public void ReturnToTitle()
    {
        PlayButtonSound();

        Controller.ReturnToTitle();
    }

    #endregion Hooks

    #region Methods

    private void SetUpCampaigns()
    {
        float yOffset = OptionMargin;
        for(int i = 0; i < _campaigns.Count; i++)
        {
            CampaignModel model = _campaigns[i];

            // Instantiate the option...
            GameObject newOption = (GameObject) Instantiate(CampaignOptionPrefab, Vector3.zero, Quaternion.identity);

            // Set newOption to be a child of the Presenter...
            newOption.transform.SetParent(OptionBindPoint.transform);

            // Update the click event on the option with the index of this campaign...
            GameObject selectButton = newOption.transform.FindChild("Select Button").gameObject;
            Button actualButton = selectButton.GetComponent<Button>();
            actualButton.onClick.AddListener(() => SelectCampaign(model));

            // Update the contents of the button with the title and description of the campaign...
            GameObject optionTitle = selectButton.transform.FindChild("Campaign Title").gameObject;
            GameObject optionDescription = selectButton.transform.FindChild("Campaign Description").gameObject;
            Text optionTitleText = optionTitle.GetComponent<Text>();
            Text optionDescriptionText = optionDescription.GetComponent<Text>();
            optionTitleText.text = model.Name;
            optionDescriptionText.text = model.Description;

            // Reposition the option in the scroll menu...
            RectTransform optionTransform = newOption.GetComponent<RectTransform>();
            Vector2 centerpoint = new Vector2(0.0f, optionTransform.rect.height / 2);
            yOffset += centerpoint.y + yOffset;
            optionTransform.anchoredPosition = new Vector3(centerpoint.x, -yOffset, 0);
        }
    }

    #endregion Methods
}
