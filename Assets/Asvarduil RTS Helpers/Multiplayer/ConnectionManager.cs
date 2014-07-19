using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ConnectionManager : ManagerBase<ConnectionManager>
{
	#region Variables / Properties

	public int GamePort = 13256;
	public float HostSearchTimeout = 5.0f;
	public float HostSearchDelay = 1.0f;

	public NetworkAddress GameServer;
	public NetworkAddress MatchmakingServer;

	public NetworkAddress NATFacilitator;
	public bool FilterNATHosts;

	public List<HostData> HostData
	{
		get { return _hostData.ToList(); }
	}

	private bool _useNAT;
	private bool _doneTestingNAT;

	private bool _isPublicIP;
	private bool _isProbingPublicIP;

	private float _timer;
	private float _lastRequest;

	private HostData[] _hostData;
	private ConnectionTesterStatus _connectionTestStatus = ConnectionTesterStatus.Undetermined;

	public bool IsConnecting { get; private set; }
	public string ErrorMessage { get; private set; }
	public string ConnectionTestMessage { get; private set; }

	#endregion Variables / Properties

	#region Engine Hooks

	public void Awake()
	{
		StartCoroutine(NetworkPretest());

		MatchmakingServer.IPAddress = MasterServer.ipAddress;
		MatchmakingServer.Port = MasterServer.port;
		NATFacilitator.IPAddress = Network.natFacilitatorIP;
		NATFacilitator.Port = Network.natFacilitatorPort;

		_hostData = MasterServer.PollHostList();
	}

	public void OnConnectedToServer()
	{
		Network.isMessageQueueRunning = false;
		GameServer.IPAddress = Network.connections[0].ipAddress;
		GameServer.Port = Network.connections[0].port;
	}
	
	public void OnDisconnectedFromServer()
	{
		if(Network.isClient)
		{
			DebugMessage("Disconnected from the Game Server.");
		}

		if(Network.isServer)
		{
			DebugMessage("Game Server has been taken offline.");
			MasterServer.UnregisterHost();
		}

		TestConnection();
	}

	public void OnFailedToConnect()
	{
		DebugMessage("Could not connect to the Game Server.");
	}

	public void OnFailedToConnectToMasterServer()
	{
		DebugMessage("Could not connect to the Matchmaking Server.");
	}

	#endregion Engine Hooks

	#region Methods

	public void StartServer(string gameName, string gameDetails)
	{
		Network.InitializeServer(1, GamePort, _useNAT);
		MasterServer.RegisterHost(gameDetails, gameName);
	}

	public void JoinGame(HostData host)
	{
		Connect(host);
	}

	public IEnumerator FindHosts(string gameName)
	{
		if(_lastRequest == 0
		   && Time.realtimeSinceStartup > _lastRequest + HostSearchTimeout)
		{
			_lastRequest = Time.time;

			MasterServer.RequestHostList(gameName);
			yield return new WaitForSeconds(HostSearchDelay);
			
			_hostData = MasterServer.PollHostList();
			yield return new WaitForSeconds(HostSearchDelay);

			DebugMessage("Search complete.  Found " + _hostData.Length + " games.");
		}
	}

	public void Connect(HostData hostInfo)
	{
		GameServer.IPAddress = hostInfo.ip[0];
		GameServer.Port = hostInfo.port;

		if(hostInfo.useNat)
			Network.Connect(hostInfo.guid);
		else
			Network.Connect(hostInfo);

		IsConnecting = true;
	}

	private IEnumerator NetworkPretest()
	{
		while(! _doneTestingNAT)
		{
			TestConnection();
			yield return 0;
		}
	}

	private void TestConnection()
	{
		_connectionTestStatus = Network.TestConnection();

		switch(_connectionTestStatus)
		{
			case ConnectionTesterStatus.Error:
				DebugMessage("Problem determining NAT capabilities.");
				_doneTestingNAT = true;
				break;

			case ConnectionTesterStatus.Undetermined:
				_doneTestingNAT = false;
				break;

			case ConnectionTesterStatus.NATpunchthroughFullCone:
			case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
				DebugMessage("NAT can punchthrough as necessary.");
				_doneTestingNAT = true;
				_useNAT = true;
				break;

			case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
				DebugMessage("Everyone except Symmetric NAT clients can connect.");
				_doneTestingNAT = true;
				_useNAT = true;
				break;
				
			case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
				DebugMessage("Only Asymmetric NAT systems are capable of NAT punchthrough!");
				_doneTestingNAT = true;
				_useNAT = true;
				break;
				
			case ConnectionTesterStatus.PublicIPIsConnectable:
				_doneTestingNAT = true;
				_useNAT = false;
				break;

			case ConnectionTesterStatus.PublicIPNoServerStarted:
				DebugMessage("Server isn't started up yet...retry when it's up!");
				break;

			// This case is a bit special as we now need to check if we can 
			// cicrumvent the blocking by using NAT punchthrough
			case ConnectionTesterStatus.PublicIPPortBlocked:
				DebugMessage("Your public IP address is blocked...");
				// If no NAT punchthrough test has been performed on this public IP, force a test
				if (! _isProbingPublicIP)
				{
					DebugMessage("Testing if firewall can be circumvented");
					_connectionTestStatus = Network.TestConnectionNAT();
					_isProbingPublicIP = true;
					_timer = Time.time + 10;
					_useNAT = false;
				}
				// NAT punchthrough test was performed but we still get blocked
				else if(Time.time > _timer)
				{
					_isProbingPublicIP = false;
					_doneTestingNAT = true;
					_useNAT = true;
				}
				break;

			default:
				break;
		}

		if(_doneTestingNAT)
		{
			DebugMessage("NAT Test is complete.");
		}
	}

	#endregion Methods
}
