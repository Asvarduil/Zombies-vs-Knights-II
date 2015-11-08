using System.Collections;
using UnityEngine;

public class TitleUIMasterController : UIMasterControllerBase<TitleUIMasterController>
{
    #region Constants

    private const string CampaignScene = "Campaign";
    private const string SkirmishScene = "Skirmish";
    private const string CommanderScene = "Commander";

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

    private TitleCommandPresenter _title;
    private TitleCommandPresenter Title
    {
        get
        {
            if (_title == null)
                _title = FindObjectOfType<TitleCommandPresenter>();

            return _title;
        }
    }

    private SettingsPresenter _settings;
    private SettingsPresenter Settings
    {
        get
        {
            if (_settings == null)
                _settings = FindObjectOfType<SettingsPresenter>();

            return _settings;
        }
    }

    #endregion Variables / Properties

    #region Hooks

    public void PresentTitle()
    {
        Settings.PresentGUI(false);

        // Since everything else is hidden, show the title.
        Title.PresentGUI(true);
    }

    public void SwitchToCampaign()
    {
        Player.RecordGameMode(GameMode.Campaign);
        StartCoroutine(FadeAndLoadScene(CampaignScene));
    }

    public void SwitchToSkirmish()
    {
        Player.RecordGameMode(GameMode.Skirmish);
        StartCoroutine(FadeAndLoadScene(SkirmishScene));
    }

    public void SwitchToCommander()
    {
        StartCoroutine(FadeAndLoadScene(CommanderScene));
    }

    public void SwitchToSettings()
    {
        Title.PresentGUI(false);
        Settings.PresentGUI(true);
    }

    public void EndGame()
    {
        StartCoroutine(FadeAndQuit());
    }

    #endregion Hooks

    #region Methods

    private IEnumerator FadeAndQuit()
    {
        //Maestro.FadeOut();
        Fader.FadeOut();

        while (!Fader.ScreenHidden)
        {
            yield return 0;
        }

        Application.Quit();
    }

    #endregion Methods
}
