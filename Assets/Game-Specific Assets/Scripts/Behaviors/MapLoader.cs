using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : JsonBlobLoaderBase<MapDetail>
{
    #region Variables / Properties

    public GameObject GameObjectContainer;
    public GameObject DoodadContainer;
    public GameObject TileContainer;

    private bool _repositoriesLoaded = false;
    private PlayerManager _player;
    private UnitRepository _unitRepository;
    private AbilityRepository _abilityRepository;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _mapper = new MapDetailMap();
        MapJsonBlob();

        _player = PlayerManager.Instance;
        var arenaFeatures = FindMapDetailForCurrentMap();
        if (arenaFeatures == null)
        {
            DebugMessage("Could not find a map detail for map " + _player.MapName);

            // TODO: Return to Hub screen.
            return;
        }

        // Don't allow any other map information to be accessed beyond this point.
        Contents = new List<MapDetail> { arenaFeatures };

        _unitRepository = UnitRepository.Instance;
        _abilityRepository = AbilityRepository.Instance;

        StartCoroutine(ArenaSetupSequence(arenaFeatures));
    }

    #endregion Hooks

    #region Methods

    private IEnumerator ArenaSetupSequence(MapDetail arenaFeatures)
    {
        DebugMessage("Arena setup initialized.");
        while (!_unitRepository.HasLoaded 
               || !_abilityRepository.HasLoaded)
        {
            yield return 0;
        }

        DebugMessage("Setting up the Arena...");
        SetupArena(arenaFeatures);
    }

    private MapDetail FindMapDetailForCurrentMap()
    {
        MapDetail model = null;

        for(int i = 0; i < Contents.Count; i++)
        {
            MapDetail current = Contents[i];
            if (current.Name != _player.MapName)
                continue;

            model = current;
            break;
        }

        return model;
    }

    private void SetupArena(MapDetail model)
    {
        PlaceObjects(model.Placements);

        // TODO: Combine all meshes.
    }

    private void PlaceObjects(List<PrefabPlacement> details)
    {
        for(int i = 0; i < details.Count; i++)
        {
            PrefabPlacement current = details[i];
            GameObject newThing = Resources.Load<GameObject>(current.Path);
            if(newThing == null)
            {
                DebugMessage("The path " + current.Path + " does not refer to a GameObject that can be loaded.");
                continue;
            }

            UnitModel unitModel = null;
            if (current.ObjectType == MapObjectType.Unit)
            {
                unitModel = _unitRepository.GetUnitByName(current.Name);
                if (unitModel == null)
                    throw new DataException("In map " + _player.MapName + ", unit " + current.Name + " is referred to; it does not exist.");
            }

            for (int j = 0; j < current.Placements.Count; j++)
            {
                PositionRotationPair placement = current.Placements[j];
                GameObject newInstance = (GameObject)Instantiate(newThing, placement.Position, Quaternion.Euler(placement.Rotation));
                newInstance.name = string.Format("{0} {1}", current.Name, j + 1);
                newInstance.transform.localScale = placement.Scale;

                // If we're dealing with a Unit, find the related model, and realize it.
                if(current.ObjectType == MapObjectType.Unit)
                {
                    UnitActuator unit = newInstance.GetComponent<UnitActuator>();
                    if (unit == null)
                        throw new DataException("In map " + _player.MapName + ", unit " + current.Name + " is not backed by a prefab with a Unit Actuator.");

                    unit.RealizeModel(unitModel);
                }

                AssociateNewThingToParent(newInstance, current.ObjectType);
            }
        }
    }

    private void AssociateNewThingToParent(GameObject newThing, MapObjectType objectType)
    {
        switch (objectType)
        {
            case MapObjectType.GameObject:
                newThing.transform.parent = GameObjectContainer.transform;
                break;

            case MapObjectType.Doodad:
                newThing.transform.parent = DoodadContainer.transform;
                break;

            case MapObjectType.Unit:
                // Do nothing.
                break;
        }
        
    }

    #endregion Methods
}
