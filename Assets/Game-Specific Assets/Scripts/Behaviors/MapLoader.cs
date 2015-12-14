using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : JsonBlobLoaderBase<MapModel>
{
    #region Constants

    public const string TerrainObjectName = "Terrain";

    #endregion Constants

    #region Variables / Properties

    public GameObject GameObjectContainer;
    public GameObject DoodadContainer;
    public GameObject TileContainer;

    private PlayerManager _player;

    private MapController _map;
    private MatchController _match;
    private GameUIMasterController _ui;
    private MapAIActuator _mapAI;

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
        Contents = new List<MapModel> { arenaFeatures };

        _ui = GameUIMasterController.Instance;
        _map = MapController.Instance;
        _match = MatchController.Instance;
        _mapAI = MapAIActuator.Instance;

        _unitRepository = UnitRepository.Instance;
        _abilityRepository = AbilityRepository.Instance;

        StartCoroutine(ArenaSetupSequence(arenaFeatures));
    }

    #endregion Hooks

    #region Methods

    private IEnumerator ArenaSetupSequence(MapModel arenaFeatures)
    {
        DebugMessage("Arena setup initialized.");
        while (!_unitRepository.HasLoaded 
               || !_abilityRepository.HasLoaded)
        {
            yield return 0;
        }

        DebugMessage("Setting up the Arena...");
        LoadAbilities(arenaFeatures);
        SetupArena(arenaFeatures);
        LoadAI(arenaFeatures.AIScriptName);

        _ui.PresentUnitCommands();
        _match.AcquireKeyUnitHPCount();
    }

    private MapModel FindMapDetailForCurrentMap()
    {
        MapModel model = null;

        for(int i = 0; i < Contents.Count; i++)
        {
            MapModel current = Contents[i];
            if (current.Name != _player.MapName)
                continue;

            model = current;
            break;
        }

        return model;
    }

    private void LoadAbilities(MapModel model)
    {
        List<string> unitAbilities = model.UnitAbilities;

        List<Ability> abilities = new List<Ability>();
        for(int i = 0; i < unitAbilities.Count; i++)
        {
            Ability unitAbility = _abilityRepository.GetAbilityByName(unitAbilities[i]);
            abilities.Add(unitAbility);

            _map.UnitSpawnAbilities = abilities.DeepCopyList();
        }
    }

    private void SetupArena(MapModel model)
    {
        SetupTerrain(model);
        PlaceObjects(model.Placements);

        // TODO: Combine all meshes.
    }

    private void SetupTerrain(MapModel model)
    {
        GameObject terrainObject = gameObject.transform.FindChild(TerrainObjectName).gameObject;

        // Set up the visible mesh...
        MeshFilter filter = terrainObject.GetComponent<MeshFilter>();
        Mesh terrainMesh = Resources.Load<Mesh>(model.Terrain.MeshPath);
        filter.mesh = terrainMesh;

        // Set up the collision mesh...
        MeshCollider collider = terrainObject.GetComponent<MeshCollider>();
        collider.sharedMesh = terrainMesh;

        // Set up materials and textures on the renderer...
        MeshRenderer renderer = terrainObject.GetComponent<MeshRenderer>();
        List<Material> materials = new List<Material>();
        for(int i = 0; i < model.Terrain.Materials.Count; i++)
        {
            MaterialModel current = model.Terrain.Materials[i];
            string materialPath = current.MaterialPath;
            Material storedMaterial = Resources.Load<Material>(materialPath);
            Material material = Instantiate(storedMaterial);

            if (!string.IsNullOrEmpty(current.DiffuseTexturePath))
            {
                Texture2D diffuseTexture = Resources.Load<Texture2D>(current.DiffuseTexturePath);
                material.SetTexture("_MainTex", diffuseTexture);
            }

            if (!string.IsNullOrEmpty(current.BumpTexturePath))
            {
                Texture2D bumpTexture = Resources.Load<Texture2D>(current.BumpTexturePath);
                material.SetTexture("_BumpMap", bumpTexture);
            }

            if (!string.IsNullOrEmpty(current.LightTexturePath))
            {
                Texture2D lightTexture = Resources.Load<Texture2D>(current.LightTexturePath);
                material.SetTexture("_EmissionMap", lightTexture);
            }

            material.mainTextureScale = current.Tiling;

            materials.Add(material);
        }

        renderer.materials = materials.ToArray();
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

    private void LoadAI(string aiScriptName)
    {
        _mapAI.LoadAI(aiScriptName);
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
