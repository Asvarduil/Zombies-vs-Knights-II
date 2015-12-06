using UnityEngine;

public class WhooshRepository : ManagerBase<WhooshRepository>
{
    #region Variables / Properties

    public GameObject HostileWhoosh;
    public GameObject NormalWhoosh;

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

    #endregion Variables / Properties

    #region Hooks

    #endregion Hooks

    #region Methods

    public GameObject GetWhooshByFaction(Faction faction)
    {
        GameObject whoosh = NormalWhoosh;

        if (faction != Player.Faction)
            whoosh = HostileWhoosh;

        return whoosh;
    }

    #endregion Methods
}

