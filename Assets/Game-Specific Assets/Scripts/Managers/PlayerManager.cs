public enum GameMode
{
    None,
    Campaign,
    Skirmish
}

public enum MatchState
{
    NotPlaying,
    OnGoing,
    Victory,
    Lost
}

public class PlayerManager : ManagerBase<PlayerManager>
{
    #region Constants

    private const string CampaignScene = "Campaign";
    private const string SkirmishScene = "Skirmish";

    #endregion Constants

    #region Variables / Properties

    public string Name = string.Empty;
    public GameMode Mode = GameMode.None;
    public string CampaignName = string.Empty;
    public Faction Faction = Faction.None;
    public string MapName = string.Empty;
    public MatchState MatchState = MatchState.OnGoing;

    // TODO: Analytics for player and developer.

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public void ClearState()
    {
        Mode = GameMode.None;
        CampaignName = string.Empty;
        MatchState = MatchState.NotPlaying;
        Faction = Faction.None;
        MapName = string.Empty;
    }

    public void RecordGameMode(GameMode mode)
    {
        Mode = mode;
    }

    public void RecordCampaignName(string campaignName)
    {
        CampaignName = campaignName;
    }

    public void Setup(string name, string mapName, Faction faction)
    {
        Name = name;
        Faction = faction;
        MapName = mapName;
        MatchState = MatchState.OnGoing;
    }

    public void RecordMatchOutcome(MatchState outcome)
    {
        // TODO: Implement further...

        DebugMessage("Recorded match outcome: " + outcome);
    }

    #endregion Methods
}
