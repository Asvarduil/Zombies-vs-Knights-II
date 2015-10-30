using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CampaignStageSelectPresenter : UGUIPresenterBase
{
    #region Variables / Properties

    public int OptionMargin;
    public List<CampaignStageModel> Stages;
    public GameObject CampaignOptionPrefab;
    public GameObject OptionBindPoint;

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
        base.Start();
    }

    public void LoadCampaign(CampaignModel campaignModel)
    {
        Stages = campaignModel.Stages.DeepCopyList();

        float yOffset = OptionMargin;
        for (int i = 0; i < Stages.Count; i++)
        {
            CampaignStageModel model = Stages[i];

            // Instantiate the option...
            GameObject newOption = (GameObject)Instantiate(CampaignOptionPrefab, Vector3.zero, Quaternion.identity);

            // Set newOption to be a child of the Presenter...
            newOption.transform.SetParent(OptionBindPoint.transform);

            // Update the click event on the option with the index of this campaign...
            GameObject selectButton = newOption.transform.FindChild("Select Button").gameObject;
            Button actualButton = selectButton.GetComponent<Button>();
            actualButton.onClick.AddListener(() => OpenStage(model));

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

        PresentGUI(true);
    }

    public void ReturnToCampaignSelect()
    {
        PlayButtonSound();

        for(int i = 0; i < OptionBindPoint.transform.childCount; i++)
        {
            GameObject child = OptionBindPoint.transform.GetChild(i).gameObject;
            ClearStageOption(child);
        }

        Controller.ViewCampaignSelect();
    }

    #endregion Hooks

    #region Methods

    private void OpenStage(CampaignStageModel model)
    {
        PlayButtonSound();
        Controller.OpenStage(model);
    }

    private void ClearStageOption(GameObject option)
    {
        DebugMessage("Clearing stage option " + option.name);
        Destroy(option);
    }

    #endregion Methods
}
