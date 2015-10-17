using System;

[Serializable]
public class ResourceStateModel
{
    #region Fields

    public Faction Faction;
    public int Count;
    public int Cap;

    public int AutoGenerateAmount;
    public Lockout AutoGenerateLockout;

    #endregion Fields

    #region Constructor

    public ResourceStateModel(Faction faction, int count = 25, int cap = 100, int autoGenAmount = 1, float lockoutRate = 1.0f)
    {
        Faction = faction;

        // TODO: Figure out better ways to initialize this all...
        Count = count;
        Cap = cap;

        AutoGenerateAmount = autoGenAmount;
        AutoGenerateLockout = new Lockout
        {
            LockoutRate = lockoutRate
        };
    }

    #endregion Constructor

    #region Methods

    public void Gain(int award)
    {
        Count += award;
        if (Count > Cap)
            Count = Cap;
    }

    public bool CanAfford(int cost)
    {
        return cost <= Count;
    }

    public void Spend(int amount)
    {
        Count -= amount;
        if (Count <= 0)
            Count = 0;
    }

    #endregion Methods
}
