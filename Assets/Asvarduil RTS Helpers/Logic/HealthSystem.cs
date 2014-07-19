using UnityEngine;
using System.Collections;

public class HealthSystem : DebuggableBehavior
{
	#region Variables / Properties

	public int HP;
	public int MaxHP;

	public float RegenInterval = 5.0f;
	public int RegenAmount = 1;

	private float _lastRegenTick;

	#endregion Variables / Properties

	#region Engine Hooks

	public void Update()
	{
		CheckRegenTick();
	}

	#endregion Engine Hooks

	#region Methods

	public void CheckRegenTick()
	{
		if(Time.time < _lastRegenTick + RegenInterval)
			return;

		Heal(RegenAmount);
		_lastRegenTick = Time.time;
	}

	public void Heal(int amount)
	{
		HP += amount;
		if(HP > MaxHP)
			HP = MaxHP;

		// TODO: Fire the 'healing' effect.
	}

	public void Damage(int amount)
	{
		HP -= amount;
		if(HP > 0)
		{
			// TODO: Fire the 'damage' effect.
			return;
		}

		// TODO: Fire the 'death' effect.
		Destroy(gameObject);
	}

	#endregion Methods
}
