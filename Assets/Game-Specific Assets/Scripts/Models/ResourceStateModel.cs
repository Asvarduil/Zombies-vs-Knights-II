using System;

[Serializable]
public class ResourceStateModel
{
    #region Fields

    public Faction Faction;
    public int Count;
    public int Cap;

    #endregion Fields

    #region Constructor

    public ResourceStateModel(Faction faction, int count = 100, int cap = 500)
    {
        Faction = faction;
        // TODO: Figure out better ways to initialize this.
        Count = count;
        Cap = cap;
    }

    #endregion Constructor

    #region Methods

    #endregion Methods
}
