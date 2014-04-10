using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer title;
    [SerializeField]
	private MeshRenderer imFirefly;
	[SerializeField]
	private MeshRenderer fireflyIsWaiting;
	[SerializeField]
	private MeshRenderer imPrincess;
	[SerializeField]
	private MeshRenderer princessIsWaiting;
	[SerializeField]
	private MeshRenderer backButton;

	private static string 		   ipServer = "127.0.0.1";
	private static readonly int portServer = 6600;

	private static readonly string ipFacilitator = "67.225.180.24";
	private static readonly int portFacilitator = 50005;

	private bool clicking;
    
	private bool networkInitialize;
    private bool mainMenu;
    private string loginPlayer;
    private string loginServer;

    private List<string> listOfPlayers;

    void Start()
    {
        Application.runInBackground = true;
		this.clicking = false;
		this.networkInitialize = false;
        this.mainMenu = true;
        this.loginPlayer = "Princess";
        this.loginServer = "Firefly";
        this.listOfPlayers = new List<string>();
    }
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			clicking = true;
		} else if (Input.GetMouseButtonUp(0)) {
			clicking = false;
		}
	}
	void FixedUpdate() {
		if (clicking) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, 100)) {
				Debug.Log ("TOUCHE : "+hit.collider.name);
				if (hit.collider.name == "IPrincessLogic" && this.mainMenu) {
					if (!princessIsWaiting.enabled) {
						WorldInfo.countNetworkConnection = Network.connections.Length + 1;
						WorldInfo.login = this.loginPlayer;
						tryToConnect();
					}
					this.mainMenu = false;
					title.enabled = false;
					imFirefly.enabled = false;
					imPrincess.enabled = false;
					princessIsWaiting.enabled = true;
					backButton.enabled = true;
				} else if (hit.collider.name == "IFireflyLogic" && this.mainMenu) {
					if (!fireflyIsWaiting.enabled) {
						WorldInfo.countNetworkConnection = Network.connections.Length + 1;
						WorldInfo.login = this.loginServer;
						Network.InitializeServer(1, 6600, true);
					}
					this.mainMenu = false;
					title.enabled = false;
					imFirefly.enabled = false;
					imPrincess.enabled = false;
					fireflyIsWaiting.enabled = true;
					backButton.enabled = true;
				} else if (hit.collider.name == "BackButtonLogic" && !this.mainMenu) {
					this.mainMenu = true;
					title.enabled = true;
					imFirefly.enabled = true;
					imPrincess.enabled = true;
					fireflyIsWaiting.enabled = false;
					princessIsWaiting.enabled = false;
					backButton.enabled = false;
					if (Network.peerType != NetworkPeerType.Disconnected) {
						Network.Disconnect();
						MasterServer.UnregisterHost();
					}
				}
			}
		}
	}
    void OnGUI()
    {
		if (this.mainMenu) {
			var configStyle = new GUIStyle();
			configStyle.normal.textColor = Color.white;
			MenuScript.ipServer = GUI.TextField(new Rect(0, Screen.height-20, 100, 20), MenuScript.ipServer);
			GUI.Label(new Rect(Screen.width-100, Screen.height-20, 100, 20), Network.player.ipAddress, configStyle);

			if (!this.networkInitialize) {
				Network.natFacilitatorPort = portFacilitator;	 
				Network.natFacilitatorIP = ipFacilitator;
				Network.InitializeSecurity();
				this.networkInitialize = true;
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
        Application.LoadLevel("MainScene");
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
