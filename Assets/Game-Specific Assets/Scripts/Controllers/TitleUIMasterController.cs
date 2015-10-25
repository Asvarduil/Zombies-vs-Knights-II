using System.Collections;
using UnityEngine;

public class TitleUIMasterController : ManagerBase<TitleUIMasterController>
{
    #region Constants

    private const string CampaignScene = "Campaign";
    private const string SkirmishScene = "Skirmish";
    private const string CommanderScene = "Commander";

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
        StartCoroutine(FadeAndLoadScene(CampaignScene));
    }

    public void SwitchToSkirmish()
    {
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
