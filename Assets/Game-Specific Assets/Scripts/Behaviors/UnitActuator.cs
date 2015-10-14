using System;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    None,
    Knights,
    Zombies
}

public class UnitActuator : DebuggableBehavior, IHealthStat
{
    #region Constants

    private const string ReticuleName = "Reticule";
    private const float KnockUpForce = 5.4f;

    #endregion Constants

    #region Variables / Properties

    private ModifiableStat _hpStat;
    public int HP
    {
        get
        {
            DefineHPStat();
            return _hpStat.Value;
        }
        private set
        {
            DefineHPStat();

            // Don't allow HP to be set less than zero.
            int newValue = value;
            if (newValue < 0)
                newValue = 0;
                
            _hpStat.Value = newValue;
        }
    }

    public int MaxHP
    {
        get
        {
            DefineHPStat();
            return _hpStat.ValueCap;
        }
    }

    public bool IsDead
    {
        get { return HP == 0; }
    }

    public string Name;
    public bool IsKeyUnit;
    public Faction Faction;
    public List<ModifiableStat> Stats;
    public List<Ability> Abilities;
    public List<Buff> Buffs;

    private string _deathObjectPath;
    public GameObject DeathObject;

    private string _meshPath;
    public GameObject UnitMeshObject;

    private UnitSelectionManager _selection;
    private MatchController _match;
    private GameEventController _gameEvent;

    private BuffRepository _buffRepository;
    private AbilityRepository _abilityRepository;

    private UnitMotion _motion;

    // TODO: Add code for telling a unit mesh to change its tint based on selection/deselection.
    private Rigidbody _rigidbody;
    private SelectionReticule _selectionReticule;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _selection = UnitSelectionManager.Instance;
        _match = MatchController.Instance;
        _gameEvent = GameEventController.Instance;

        _buffRepository = BuffRepository.Instance;
        _abilityRepository = AbilityRepository.Instance;

        _motion = GetComponent<UnitMotion>();
        _rigidbody = GetComponent<Rigidbody>();
        _selectionReticule = transform.FindChild(ReticuleName).GetComponent<SelectionReticule>();
    }

    public void Update()
    {
        ProcessBuffs();
    }

    public void OnMouseEnter()
    {
        _selection.UpdateCursor(this);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Collider who = collision.collider;

        // Get the associated unit.  If the collidee is not a unit, do nothing.
        UnitActuator unit = who.gameObject.GetComponent<UnitActuator>();
        if (unit == null)
        {
            DebugMessage(who.gameObject.name + " is not a unit; no collision-combat occurred.");
            return;
        }

        // If the unit is friendly do nothing.
        if (unit.Faction == Faction)
        {
            DebugMessage("Unit " + unit.Name + " is friendly to " + Name + "; no collision-combat occurred.");
            return;
        }

        // Otherwise, deal damage and repel the other unit.
        int damage = Stats.FindItemByName("Attack").ModifiedValue;
        int knockback = Stats.FindItemByName("Knockback").ModifiedValue;
        unit.TakeDamage(damage);
        unit.TakeKnockback(knockback);
    }

    #endregion Hooks

    #region Methods

    private void DefineHPStat()
    {
        if (_hpStat == null)
            _hpStat = Stats.FindItemByName("HP");

        if (_hpStat == default(ModifiableStat))
            throw new DataException("Unit " + Name + " has no HP stat defined.");
    }

    public void Heal(int amount)
    {
        HP += amount;
        if (HP > MaxHP)
            HP = MaxHP;

        // TODO: Healy things.
    }

    public void TakeDamage(int amount)
    {
        DebugMessage("Unit " + Name + " is taking " + amount + " damage.");

        HP -= amount;
        DebugMessage("Unit " + Name + "'s HP is now " + HP);

        if (! IsDead)
            return;
        
        DebugMessage("Unit " + Name + " has been defeated.");

        if (DeathObject != null)
        {
            DebugMessage("Spawning death object at current position...");
            Instantiate(DeathObject, transform.position, transform.rotation);
        }

        DebugMessage("Checking if this unit's defeat should end the match...");
        _match.CheckForMatchConclusion();

        DebugMessage("Disabling game object...");
        gameObject.SetActive(false);
    }

    public void TakeKnockback(float knockbackForce)
    {
        // Not all units have rigidbody physics...
        if (_rigidbody == null)
            return;

        DebugMessage("Unit " + Name + " is getting knocked back with a force of " + knockbackForce + "N.");

        // If no knockback, it won't have any effect anyways, do nothing.
        if (Mathf.Abs(knockbackForce - 0.0f) < 0.001f)
            return;

        // Apply it.
        Vector3 forceVector = new Vector3(0, KnockUpForce, knockbackForce);
        _rigidbody.AddRelativeForce(forceVector, ForceMode.Impulse);
    }

    public void RealizeModel(UnitModel model)
    {
        Name = model.Name;
        IsKeyUnit = model.IsKeyUnit;
        Faction = model.Faction;
        Stats = model.Stats.DeepCopyList();

        Abilities = new List<Ability>();
        for (int i = 0; i < model.AbilityNames.Count; i++)
        {
            string currentName = model.AbilityNames[i];

            if (_abilityRepository == null)
                _abilityRepository = AbilityRepository.Instance;

            Ability actualAbility = _abilityRepository.GetAbilityByName(currentName);
            if (actualAbility == null)
            {
                DebugMessage("Could not map ability " + currentName + " because it doesn't exist in the Ability Repository.");
                continue;
            }

            Abilities.Add(actualAbility.DeepCopy());
        }

        Buffs = new List<Buff>();

        _deathObjectPath = model.DeathObjectPath;
        if(! string.IsNullOrEmpty(_deathObjectPath))
            DeathObject = Resources.Load<GameObject>(_deathObjectPath);

        _meshPath = model.MeshPath;
        if (!string.IsNullOrEmpty(_meshPath))
        {
            GameObject unitMesh = Resources.Load<GameObject>(_meshPath);
            
            UnitMeshObject = (GameObject) Instantiate(unitMesh, transform.position, transform.rotation);
            UnitMeshObject.name = "Mesh";
            UnitMeshObject.transform.parent = gameObject.transform;

            MeshCollider collider = gameObject.AddComponent<MeshCollider>();
            collider.enabled = true;
            collider.convex = true;
            collider.sharedMesh = UnitMeshObject.GetComponent<MeshFilter>().mesh;
        }

        Transform reticuleTransform = transform.Find(ReticuleName);
        reticuleTransform.localScale = Vector3.one * model.ReticuleScale;
    }

    public void ApplyBuff(string buffName)
    {
        Buff newBuff = _buffRepository.GetBuffByName(buffName);
        if (newBuff == null)
            return;

        Buffs.Add(newBuff.DeepCopy());
        ApplyBuffEffect(newBuff);
    }

    private void ProcessBuffs()
    {
        float currentTime = Time.time;
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buff current = Buffs[i];

            if (current.HasExpired(currentTime))
            {
                RemoveBuffEffect(current);
                continue;
            }

            if (current.TickHasOccurred(currentTime))
            {
                ApplyBuffEffect(current);
            }
        }
    }

    private void RemoveBuffEffect(Buff buff)
    {
        ModifiableStat stat = Stats.FindItemByName(buff.AffectedStat);
        // TODO: When I figure out how to do this in a non-glitchy way.
    }

    private void ApplyBuffEffect(Buff buff)
    {
        ModifiableStat stat = Stats.FindItemByName(buff.AffectedStat);
        // TODO: When I figure out how to do this in a non-glitchy way.
        //stat.AddTemporaryBonus(buff.EffectValue);
    }

    public void DeselectUnit()
    {
        _selectionReticule.ChangeAppearance(SelectionState.Hidden);
    }

    public void SelectUnit(Faction playerFaction)
    {
        SelectionState state = Faction == playerFaction
            ? SelectionState.Friendly
            : SelectionState.Enemy;

        DebugMessage(gameObject.name + " is selected as a(n) " + state);
        _selectionReticule.ChangeAppearance(state);
    }

    public void IssueCommand(AbilityCommmandTrigger command, UnitActuator targetUnit = null)
    {
        if (_motion == null)
            return;

        for(int i = 0; i < Abilities.Count; i++)
        {
            Ability current = Abilities[i];
            if(current.AbilityCommandTrigger != command)
            {
                FormattedDebugMessage(LogLevel.Information,
                    "Unit {0} not executing ability {1} - command {2} does not match that of the ability.",
                    Name,
                    current.Name,
                    current.AbilityCommandTrigger);
                continue;
            }

            if(!string.IsNullOrEmpty(current.EffectPath))
            {
                GameObject effect = Resources.Load<GameObject>(current.EffectPath);
                GameObject actualEffect = (GameObject) Instantiate(effect, transform.position, transform.rotation);

                // TODO: Determine how to handle effects that expire on a state change.
            }

            _gameEvent.RunGameEventGroup(current.GameEvents);
        }

        // Signal UnitMotion to actually move the unit...
        switch (command)
        {
            case AbilityCommmandTrigger.MoveTo:
                if (targetUnit == null)
                {
                    _motion.Halt();
                    break;
                }

                DebugMessage("Self was selected as target; holding position and defending.");
                _motion.SetTarget(targetUnit.gameObject);
                break;

            case AbilityCommmandTrigger.Defend:
                _motion.Halt();
                break;

            case AbilityCommmandTrigger.MatchOver:
                _motion.Halt();
                break;

            default:
                throw new InvalidOperationException("Unexpected command: " + command);
        }
    }

    #endregion Methods
}
