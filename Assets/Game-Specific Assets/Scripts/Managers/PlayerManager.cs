public enum MatchState
{
    OnGoing,
    Victory,
    Lost
}

public class PlayerManager : ManagerBase<PlayerManager>
{
    #region Variables / Properties

    public string Name;
    public Faction Faction;
    public string MapName;
    public MatchState MatchState;
    // TODO: Faction Resources

    // TODO: Analytics for player and developer.

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

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
