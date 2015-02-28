using UnityEngine;
using System.Collections;

public class MultiplayerScript : MonoBehaviour {

	private string titleMessage = "Prototype";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26500;
	private bool useNAT = false;
	private string ipAddress;
	private string port;
	private int numberOfPlayers = 10;
	
	public string playerName;
	public string serverName;
	public string serverNameForClient;
	
	private bool iWantToSetupAServer = false;
	private bool iWantToConnectToAServer = false;
	public bool serverStartedAndNoPlayersConnected = false;
	
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndendt;
	private int topIndent;
	
	private Rect serverDisWindowRect;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;
	
	private Rect clientDisWindowRect;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;
	public bool showDisconnectWindow = false;
	
	// Use this for initialization
	void Start () {
		
		serverName = PlayerPrefs.GetString("serverName");
		if(serverName == "")
		{
			serverName = "Server";
		}
		
		playerName = PlayerPrefs.GetString("playerName");
		if(playerName == "")
		{
			playerName = "Player";
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			showDisconnectWindow = !showDisconnectWindow;
		}
		
		if(GameObject.Find("SpawnManager").GetComponent<SpawnScript>().iAmDestroyed == true)
		{
			showDisconnectWindow = false;
		}
	}
	
	void ConnectWindow(int windowID)
	{
		GUILayout.Space(15);
		
		if(iWantToSetupAServer == false && iWantToConnectToAServer == false)
		{
			if(GUILayout.Button("Setup a server", GUILayout.Height(buttonHeight)))
			{
				iWantToSetupAServer = true;
			}
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight)))
			{
				iWantToConnectToAServer = true;
			}
			
			GUILayout.Space(10);
			
			if(Application.isWebPlayer == false && Application.isEditor == false)
			{
				if(GUILayout.Button("Exit Prototype", GUILayout.Height(buttonHeight)))
				{
					Application.Quit();
				}
			}
		}
		
		if(iWantToSetupAServer == true)
		{
			GUILayout.Label("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);
			
			GUILayout.Space(5);
			
			GUILayout.Label("Server Port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Start my own server", GUILayout.Height(30)))
			{
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);
				
				PlayerPrefs.SetString("serverName", serverName);
				
				iWantToSetupAServer = false;

				serverStartedAndNoPlayersConnected = true;
			}
			
			if(GUILayout.Button("Go Back", GUILayout.Height(30)))
			{
				iWantToSetupAServer = false;
			}
		}
		
		if(iWantToConnectToAServer == true)
		{
			GUILayout.Label("Enter your player name");
			playerName = GUILayout.TextField(playerName);
			
			GUILayout.Space(5);
			
			GUILayout.Label ("Type in Server IP");
			connectToIP = GUILayout.TextField(connectToIP);
			
			GUILayout.Space(5);
			
			GUILayout.Label("Server Port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(5);
			
			if(GUILayout.Button("Connect", GUILayout.Height(25)))
			{
				if(playerName == "")
				{
					playerName = "Player";
				}
				if(playerName != "")
				{
					Network.Connect(connectToIP, connectionPort);
					PlayerPrefs.SetString("playerName", playerName);
					//GameObject.Find("GameManager").GetComponent<GameManager>().SpawnPlayer();
				}
				
			}
			
			GUILayout.Space(5);
			
			if(GUILayout.Button("Go Back", GUILayout.Height(25)))
			{
				iWantToConnectToAServer = false;
			}			
		}
		
	}
	
	void ServerDisconnectWindow(int windowID)
	{
		GUILayout.Label("Server name: " + serverName);
		GUILayout.Label("Number of players connected: " + Network.connections.Length);
		
		if(Network.connections.Length >= 1)
		{
			GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
		}
		if(GUILayout.Button("Shutdown server"))
		{
			Network.Disconnect();
		}
		
	}
	
	void ClientDisconnectWindow(int windowID)
	{
		GUILayout.Label("Connected to server: " + serverName);
		GUILayout.Label("Ping: " + Network.GetAveragePing(Network.connections[0]));
		
		GUILayout.Space(7);
		
		if(GUILayout.Button("Disconnect", GUILayout.Height(25)))
		{
			Network.Disconnect();
		}
		
		GUILayout.Space(5);
		
		if(GUILayout.Button("Return To Game", GUILayout.Height(25)))
		{
			showDisconnectWindow = false;
		}
	}
	
	void OnDisconnectedFromServer()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		Network.RemoveRPCs(networkPlayer);
		
		Network.DestroyPlayerObjects(networkPlayer);
	}
	
	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		networkView.RPC("TellPlayerServerName", networkPlayer, serverName);
	}
	
	void OnGUI()
	{
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			leftIndendt = Screen.width / 2 - connectionWindowWidth / 2;
			topIndent = Screen.height / 2 - connectionWindowHeight / 2;
			
			connectionWindowRect = new Rect(leftIndendt, topIndent, connectionWindowWidth, connectionWindowHeight);
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titleMessage);
		}
		
		if(Network.peerType  == NetworkPeerType.Server)
		{
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindowRect = GUILayout.Window(1, serverDisWindowRect, ServerDisconnectWindow, "");
		}
		
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
		{
			clientDisWindowRect = new Rect(Screen.width / 2 - clientDisWindowWidth / 2, Screen.height / 2 - clientDisWindowHeight / 2,
			                               clientDisWindowWidth, clientDisWindowHeight);
			clientDisWindowRect = GUILayout.Window(1, clientDisWindowRect, ClientDisconnectWindow, "");
		}
	}
	
	[RPC]
	void TellPlayerServerName (string servername)
	{
		serverName = servername;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
}
