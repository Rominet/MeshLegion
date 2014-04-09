using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private Texture logo;

	private static string 		   ipServer = "127.0.0.1";
	private static readonly int portServer = 6600;

	private static readonly string ipFacilitator = "67.225.180.24";
	private static readonly int portFacilitator = 50005;
    
	private bool networkInitialize;
    private bool mainMenu;
	private bool princess;
	private bool firefly;
    private string loginPlayer;
    private string loginServer;

    private List<string> listOfPlayers;
    void Start()
    {
        Application.runInBackground = true;
		this.networkInitialize = false;
        this.mainMenu = true;
        this.princess = false;
		this.firefly = false;
        this.loginPlayer = "Princess";
        this.loginServer = "Firefly";
        listOfPlayers = new List<string>();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - logo.width / 3 - 50, 0, 470, 200), logo);
		if (!this.mainMenu) {//Network.peerType == NetworkPeerType.Disconnected) {
			WorldInfo.countNetworkConnection = Network.connections.Length + 1;
			if (this.firefly) {
				var configStyle = new GUIStyle ();
				configStyle.normal.textColor = Color.black;   
				GUI.Label (new Rect (Screen.width * 0.5f-200.0f, Screen.height * 0.5f, 450, 40), "Vous commencez à chercher la princesse dans ce grand chateau...", configStyle);
			} else if (this.princess) {
				var configStyle = new GUIStyle ();
				configStyle.normal.textColor = Color.black;   
				GUI.Label (new Rect (Screen.width * 0.5f-200.0f, Screen.height * 0.5f, 450, 40), "Vous pleurez dans votre coin en attendant que l'on vienne vous aider...", configStyle);
			}
			if (GUI.Button(new Rect(Screen.width * 0.5f-70.0f, Screen.height * 0.5f+60, 140, 30), "Retour"))
			{
				mainMenu = true;
				if (Network.peerType != NetworkPeerType.Disconnected) {
					Network.Disconnect();
					MasterServer.UnregisterHost();
				}
			}
		} else {
			var configStyle = new GUIStyle();
			configStyle.normal.textColor = Color.black;
			MenuScript.ipServer = GUI.TextField(new Rect(0, Screen.height-20, 100, 20), MenuScript.ipServer);
			GUI.Label(new Rect(Screen.width-100, Screen.height-20, 100, 20), Network.player.ipAddress, configStyle);

			if (!this.networkInitialize) {
				Network.natFacilitatorPort = portFacilitator;	 
				Network.natFacilitatorIP = ipFacilitator;
				Network.InitializeSecurity();
				this.networkInitialize = true;
			}

            if (GUI.Button(new Rect(Screen.width * 0.5f - 140, Screen.height * 0.5f - 30, 280, 30), "Play the Princess"))
            {
				this.mainMenu = this.firefly = false;
				this.princess = true;
				WorldInfo.login = this.loginServer;
				Network.InitializeServer(1, 6600, true);
            }
			if (GUI.Button(new Rect(Screen.width * 0.5f - 140, Screen.height * 0.5f + 30, 280, 30), "Play the Firefly"))
            {
				this.mainMenu = this.princess = false;
				this.firefly = true;
				WorldInfo.login = this.loginPlayer;
				tryToConnect();
            }
        }
    }

	private void tryToConnect() {
		if (Network.peerType == NetworkPeerType.Disconnected) {
			Debug.Log ("[MENUSCRIPT] tryToConnect");
			Network.Connect (MenuScript.ipServer, MenuScript.portServer);
		}
	}

    [RPC]
    private void loginReception(string login)
    {
        if (Network.isServer && login != null)
        {
			Debug.Log("[MENUSCRIPT] loginReception: <" + login + "> catched");
            this.listOfPlayers.Add(login);
            string[] listPlayers = new string[listOfPlayers.Count];
            for (int i = 0; i < listOfPlayers.Count; i++)
                listPlayers[i] = listOfPlayers[i];
            this.networkView.RPC("loginReception", RPCMode.Others, string.Join(":", listPlayers));
			this.networkView.RPC("launchGame", RPCMode.All);
        }
        else
        {
            string[] logins = login.Split(':');
            for (int i = 0; i < logins.Length; ++i)
                this.listOfPlayers.Add(logins[i]);
        }
    }

    [RPC]
    void launchGame()
    {
        Application.LoadLevel("SimpleMazeScene");
    }

	void OnFailedToConnect(NetworkConnectionError error) {
		Debug.Log("Could not connect to server: " + error+", retrying in 3 seconds");
		InvokeRepeating("tryToConnect", 0, 3);
	}

    void OnConnectedToServer()
    {
        this.networkView.RPC("loginReception", RPCMode.Server, this.loginPlayer);
    }

    void OnServerInitialized(){
        this.listOfPlayers.Add(this.loginPlayer);
    }
}
