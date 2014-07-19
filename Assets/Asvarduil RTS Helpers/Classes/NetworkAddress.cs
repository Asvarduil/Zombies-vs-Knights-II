using System;

[Serializable]
public class NetworkAddress
{
	#region Variables / Properties

	public string IPAddress;
	public int Port;

	#endregion Variables / Properties

	#region Methods

	public override string ToString ()
	{
		return IPAddress + ":" + Port;
	}

	#endregion Methods
}
