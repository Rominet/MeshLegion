﻿//#define DEBUG_DISABLED_NETWORK

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class serializableInformations
{
    public object[] clientsInfos;
    public object[] worldInfos;
}

public class NetworkManager : MonoBehaviour {
    //private static readonly string SERVERIP = "172.19.113.24";
	//private static readonly int SERVERPORT = 6600;


    //Mario, worms, dk, zelda, pokemon, pacman
    /*private Vector3[] playerSpawn = new Vector3[] { new Vector3(5f, -1.6f, -8.9f), new Vector3(-5f,  -1.6f,  0.8f)
                                                  , new Vector3(5f, -1.6f,   0.8f), new Vector3(-5f, -1.6f, -8.9f) };
	*/
	private Vector3 playerSpawn = new Vector3(0, 0, 0);
	//private bool inGame;
	private NetworkView nV;
    //private string ownLogin;

    //Clients
	private Dictionary<string, ClientInfo> clients;
    private Dictionary<string, int> positionsClients;
    private int countClients;

    private static NetworkManager instance;

    public static NetworkManager getInstance() { return instance; }
    public Dictionary<string, ClientInfo> getClients() { return this.clients; }

    void Start()
    {
        Debug.Log("[NETWORK MANAGER] start: i'm <"+WorldInfo.login+">");
        instance = this;
        this.nV = this.gameObject.networkView; //.GetComponent<NetworkView>();
        this.clients = new Dictionary<string, ClientInfo>();
        this.positionsClients = new Dictionary<string, int>();
        this.countClients = 0;
        //this.inGame = true;

        //Get login from this client from screen connection
        if (Network.isServer) {
            //Add server to the client list, because the server is a client too AHAHAHA in SixLegend, yes, but not in this game ! 
            //this.addNewClient(WorldInfo.login);
        } else {
            this.nV.RPC("loginReception", RPCMode.Server, WorldInfo.login);
        }
    }

    [RPC]
    private void loginReception(string login)
    {
        if (Network.isServer && login != null) {
            Debug.Log("[NETWORK MANAGER] loginReception: <" + login + "> catched");
            this.addNewClient(login);
        }
    }

    private void addNewClient(string login)
    {
        Debug.Log("[NETWORK MANAGER] addNewClient: new client <" + login + "> added to the client list");
		GameObject g = null;
		g = WorldInfo.getClientGO();
        if (g == null) {
			Debug.LogError("[NETWORK MANAGER] addNewClient: getClientGO fail !");//pool doesn't have enough players
            return;
        }

        ClientInfo c = g.AddComponent("ClientInfo") as ClientInfo;
        if (c == null) {
            Debug.LogError("[NETWORK MANAGER] addNewClient: clientInfo was not created");
            return;
        }
        
        this.clients.Add(login, c);
    }

    public void clientInfoIsReady(string login, ClientInfo c)
    {
        if (Network.isServer) {
            this.positionsClients[login] = this.countClients;
        }
        Debug.Log("[NETWORK MANAGER] clientInfoIsReady: <" + login + "> was created at position <" + this.positionsClients[login] + ">");
        c.transform.Translate(this.playerSpawn);
        c.setNum(this.positionsClients[login]);
        
        WorldInfo.listOfPlayers.Add(login);
        this.countClients++;

        int nbPlayers = this.countClients;
        Debug.Log(WorldInfo.countNetworkConnection + " == " + nbPlayers);
        if (WorldInfo.countNetworkConnection == nbPlayers) {
            if (!Network.isServer) {
                Debug.Log("[NETWORK MANAGER] clientInfoIsReady: PLAYERS LOGINS ARE DISPACHED, SEND TO SERVER THAT I'M READY");
                this.nV.RPC("clientIsReady", RPCMode.Server, WorldInfo.login);
                return;
            } else {
                Debug.Log("[NETWORK MANAGER] clientInfoIsReady: SERVER IS READY, DISPATCHING TO OTHER ALL PLAYER LOGINS [SERVER]");
                this.clients[WorldInfo.login].setState("client ready confirmed");
                string[] logins = new string[this.countClients];
                string[] positions = new string[this.countClients];
                for (int i = 0; i < this.countClients; ++i) {
                    logins[i] = WorldInfo.listOfPlayers[i];
                    positions[i] = this.positionsClients[WorldInfo.listOfPlayers[i]].ToString();
                }
                this.nV.RPC("serverIsReady", RPCMode.Others, string.Join(":", logins), string.Join(":", positions));
            }
        }
    }
    void OnGUI()
    {
        int i = 0;
        foreach (KeyValuePair<string, ClientInfo> infos in clients)
        {
            GUI.Label(new Rect(125, 40 * i, 450, 40), "Joueur : " + infos.Key + " Score : " + infos.Value.getScore());
            ++i;
        }
    }

    [RPC]
    public void serverIsReady(string loginsJoin, string positionsJoin)
    {
        Debug.Log("[NETWORK MANAGER] serverIsReady");
        if (Network.isServer)
            return;

        string[] logins = loginsJoin.Split(':');
        string[] positions = positionsJoin.Split(':');

        int count = logins.Length;
        for (int i = 0; i < count; ++i) {
            try {
                this.positionsClients[logins[i]] = int.Parse(positions[i]);
            } catch (Exception) {
                Debug.LogError("[NETWORK MANAGER] serverIsReady: impossible de convertir <" + positions[i] + "> en int");
                return;
            }
            this.addNewClient(logins[i]);
        }
    }

    [RPC]
    public void clientIsReady(string login)
    {
        //Debug.Log("[NETWORK MANAGER] clientIsReady [SERVER]");
        if (!Network.isServer)
            return;

        if (this.realClient(login)) {
            this.clients[login].setState("client ready confirmed");
            //Debug.Log("[NETWORK MANAGER] clientIsReady: client <" + login + "> is READY TO PLAY [SERVER]");
        }

        int nbPlayers = WorldInfo.listOfPlayers.Count;
        for (int i = 0; i < nbPlayers; ++i)
            if (this.clients[WorldInfo.listOfPlayers[i]].getState() != "client ready confirmed")
                return;

        //Debug.Log("[NETWORK MANAGER] clientIsReady: ALL THE CLIENTS HAVE CONFIRMED THAT THEY ARE READY TO PLAY [SERVER]");


        InvokeRepeating("forceSendWishes", 0, 3.0F); 

        //GO TO PLAYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY
        this.nV.RPC("setWorldState", RPCMode.Others, "PLAY");

        WorldInfo.state = "PLAY";
        //Debug.Log("[NETWORK MANAGER] clientInfoIsReady: GOOOO TO PLAAAAAAY [SERVER]");
    }

    [RPC]
    public void setWorldState(string state)
    {
        Debug.Log("[NETWORK MANAGER] setWorldState: <"+state+"> [SERVER]");
        WorldInfo.state = state;
    }

    [RPC]
    public void updateScore(string login)
    {
        Debug.Log("[ UPDATE SCORE ] " + login);
        if(Network.isServer){
            this.clients[login].setScore();
            this.networkView.RPC("updateScore",RPCMode.Others,login);
        }
        else
            this.clients[login].setScore();
    }

    public string getLoginOfGameObjectPlayer(GameObject g) {
        foreach (KeyValuePair<string, ClientInfo> client in this.clients)
            if (client.Value.gameObject == g)
                return client.Key;
        return null; 
    }

    private bool realClient(string login, string pass = null)
    {
        if (!this.clients.ContainsKey(login))
            return false;

        //if (login && pass ARE GOOD)
            return true;
    }


	// Update is called once per frame
	void Update () {}

    public void sendWishes(string login, byte[] infos, bool state, int type) {
        if (Network.isServer) {
            //this.dispatchWishes(login, infos, state, type);
            //Debug.Log("[NETWORK MANAGER] sendWishes: wishes <" + login + "> local TO NETWORK [SERVER]");
            this.nV.RPC("dispatchWishes", RPCMode.Others, login, infos, state, type);
        } else {
            //Debug.Log("[NETWORK MANAGER] sendWishes: wishes <" + login + "> local TO NETWORK");
            this.nV.RPC("dispatchWishes", RPCMode.Server, WorldInfo.login, infos, state, type);
        }
    }

    /**
     * Types:
     *      0 : ClientInfo
     *      1 : WorldInfo
     *      
     *      99: ALL
     */
    [RPC]
    private void dispatchWishes(string callerLogin, byte[] infos, bool state, int type)
    {
        //If it's a state, all clients take the informations even the player himself
        //Else, others informations get from the server can't act on the player himself

        if (callerLogin == null || callerLogin == "") {
            Debug.LogError("[NETWORK MANAGER] dispatchWishes: callerLogin doesn't exist");
            return;
        }

        //NEW PLAYER....but we
        /*if (!this.clients.ContainsKey(callerLogin)) {
            Debug.LogError("[NETWORK MANAGER] dispatchWishes: <" + callerLogin + "> is an UNKNOWN CLIENT ");
            //this.addNewClient(callerLogin);
            return;
        } 
        else */if (type != 0 || state || WorldInfo.login != callerLogin)
        {
            //Debug.Log("[NETWORK MANAGER] dispatchWishes: <" + callerLogin + "> from network TO LOCAL of type <"+type+">");
            switch (type) {
                case 0: //ClientInfo
                    this.clients[callerLogin].receptWishesFromServer(infos);
                    break;

            }
            
        }
    }
    public void clientsGoToSpawn()
    {
        Debug.Log("[NETWORK MANAGER] clientsGoToSpawn");
        string log = null;
        for (int i = 0; i < this.countClients; ++i) {
            log = WorldInfo.listOfPlayers[i];
			this.clients[log].transform.position = this.playerSpawn;//[this.positionsClients[log]];
        }
    }
	
	///* TODO: TOUTE LES SECONDES ACTUALISER INFORMATIONS SUR LES CLIENTS (positions des joueurs, les morts, les caisses
    ///

    private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {}

    public void forceSendWishes()
    {
        if (Network.isClient)
            return;

        //Sending of the first wishes from all player gameobject on the server to all client's player gameobject
        //Debug.Log("[NETWORK MANAGER] forceSendWishes: SENDING: PLAYERS [SERVER]");
        int nbPlayers = WorldInfo.listOfPlayers.Count;
        for (int i = 0; i < nbPlayers; ++i) {
            //Debug.Log("[NETWORK MANAGER] forceSendWishes: local TO NETWORK:  <" + WorldInfo.listOfPlayers[i] + "> is READY TO PLAY [SERVER]");
            this.clients[WorldInfo.listOfPlayers[i]].sendWishesToServer(true);
        }
    }
    public static byte[] objectToByte(object src)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, src);

        return ms.ToArray();
    }
	public static object byteToObject(byte[] src)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(src);
        
        return bf.Deserialize(ms);
    }
}