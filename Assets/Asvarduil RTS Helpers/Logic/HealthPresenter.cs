using UnityEngine;
using System.Collections;

public class HealthPresenter : PresenterBase
{
	#region Variables / Properties

	public bool ShowHealth = false;

	public FloatingTexture Background;
	public FloatingTexture Mask;

	public Texture2D FriendlyActiveTick;
	public Texture2D EnemyActiveTick;
	public Texture2D NeutralActiveTick;
	public Texture2D InactiveTick;
	public Vector2 MeterDimensions;
	public FloatingTexture Meter;

	private HealthSystem _health;
	private UnitFaction _faction;
	private Texture2D _healthBarComposite;

	#endregion Variables / Properties

	#region Hooks

	public void SelectMe(Teams team)
	{
		ShowHealth = true;
		Recalculate();
	}

	public void DeselectMe()
	{
		ShowHealth = false;
	}

	public override void Start()
	{
		base.Start();
		_health = GetComponent<HealthSystem>();
		_faction = GetComponent<UnitFaction>();

		_healthBarComposite = new Texture2D((int) MeterDimensions.x, (int) MeterDimensions.y);
		Recalculate();
	}

	public void Recalculate()
	{
		int canvasWidth = InactiveTick.width * _health.MaxHP;
		int canvasHeight = InactiveTick.height;
		int fullTicks = _health.HP;
		int deadTicks = _health.MaxHP - _health.HP;
		int tickX = 0;
		
		Texture2D tex = new Texture2D(canvasWidth, canvasHeight);
		DrawFullTicks(fullTicks, tex, ref tickX);
		DrawEmptyTicks(deadTicks, tex, ref tickX);
		
		tex.Apply();
		_healthBarComposite = tex;
		Meter.image = _healthBarComposite;
	}

	private void DrawFullTicks(int fullTicks, Texture2D tex, ref int tickX)
	{
		Texture2D ActiveTick;
		if(_faction == null
		   ||_faction.Team == Teams.Spectator
		   || TeamManager.Instance.PlayerTeam == Teams.Spectator)
		{
			ActiveTick = NeutralActiveTick;
		}
		else if(TeamManager.Instance.PlayerTeam == _faction.Team)
		{
			ActiveTick = FriendlyActiveTick;
		}
		else
		{
			ActiveTick = EnemyActiveTick;
		}

		for(int counter = 0; counter < fullTicks; counter++)
		{
			tex.SetPixels(tickX, 0, ActiveTick.width, ActiveTick.height, ActiveTick.GetPixels());
			tickX += ActiveTick.width;
		}
	}
	
	private void DrawEmptyTicks(int deadTicks, Texture2D tex, ref int tickX)
	{
		for(int counter = 0; counter < deadTicks; counter++)
		{
			tex.SetPixels(tickX, 0, InactiveTick.width, InactiveTick.height, InactiveTick.GetPixels());
			tickX += InactiveTick.width;
		}
	}

	public override void SetVisibility(bool isVisible)
	{
		//float opacity = DetermineOpacity(isVisible);
	}

	public override void DrawMe()
	{
		if(! ShowHealth)
			return;

		Background.DrawMe();
		Meter.DrawMe();
		Mask.DrawMe();
	}

	public override void Tween()
	{
		Background.CalculatePosition(transform.position);
		Meter.CalculatePosition(transform.position);
		Mask.CalculatePosition(transform.position);
	}

	#endregion Hooks
}
