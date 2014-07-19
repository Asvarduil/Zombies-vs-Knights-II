using System;
using UnityEngine;

[Serializable]
public class AsvarduilControlAxis
{
	#region Variables / Properties

	public string Name;
	public string PositiveKey;
	public string NegativeKey;
	public bool IsInverted = false;
	public bool Sharp = false;
	public float DeadZone = 0.1f;
	public float Sensitivity = 1.0f;

	private float _value;

	#endregion Variables / Properties

	#region Methods

	public bool GetPositiveKey()
	{
		return Input.GetKey(PositiveKey);
	}

	public bool GetNegativeKey()
	{
		return Input.GetKey(NegativeKey);
	}

	public float GetAxis()
	{
		switch(Application.platform)
		{
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsWebPlayer:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.OSXDashboardPlayer:
				return GetKey();

			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
			case RuntimePlatform.WP8Player:
				throw new Exception("Key-based Axes not supported on mobile platforms!");

			default:
				throw new Exception("Platform not supported: " + Application.platform);
		}
	}

	public float GetKeyDown()
	{
		if(!string.IsNullOrEmpty(PositiveKey)
		   && Input.GetKeyDown(PositiveKey))
		{
			_value = IsInverted 
				? Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity)
				: Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity);
		}
		else if(!string.IsNullOrEmpty(NegativeKey)
		        && Input.GetKeyDown(NegativeKey))
		{
			_value = IsInverted 
				? Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity)
				: Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity);
		}
		
		return _value <= DeadZone ? 0.0f : _value;
	}

	public float GetKeyUp()
	{
		if(!string.IsNullOrEmpty(PositiveKey)
		   && Input.GetKeyUp(PositiveKey))
		{
			_value = IsInverted 
				? Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity)
					: Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity);
		}

		if(!string.IsNullOrEmpty(NegativeKey)
		   && Input.GetKeyUp(NegativeKey))
		{
			_value = IsInverted 
				? Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity)
					: Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity);
		}
		
		return _value <= DeadZone ? 0.0f : _value;
	}

	private float GetKey()
	{
		if(!string.IsNullOrEmpty(PositiveKey)
		   && Input.GetKey(PositiveKey))
		{
			_value = IsInverted 
				     ? Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity)
					 : Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity);
		}
		else if(!string.IsNullOrEmpty(NegativeKey)
		        && Input.GetKey(NegativeKey))
		{
			_value = IsInverted 
				     ? Sharp ? 1.0f : Mathf.Lerp(_value, 1.0f, Sensitivity)
					 : Sharp ? 0.0f : Mathf.Lerp(_value, 0.0f, Sensitivity);
		}
		
		return _value <= DeadZone ? 0.0f : _value;
	}

	#endregion Methods
}
