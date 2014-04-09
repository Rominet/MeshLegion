using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class clientInformationsSerializable
{
    public bool[] activateDirections;
	public bool activateFire;
	public Quaternion rotateQuater;
}


public class ClientInfo : MonoBehaviour, NetworkInterface
{
	//Singleton
	private static ClientInfo instance;
	public static ClientInfo getInstance() { return instance; }
	
	
	private static readonly Vector3[] directions = new[] { Vector3.forward, Vector3.back};
    private static readonly float SPEED = 4.0f;

    private NetworkManager netWManager;

    private int num;
    private bool hasWishes;
    private string login;
    private string state;
    private int score;

    //Player
    private bool[] activateDirections;
	private bool activateFire;
	private Quaternion rotateQuater;

    //private GameObject gameObjectPlayer;

    void Start()
    {
        this.netWManager = NetworkManager.getInstance();
        
		this.login = this.netWManager.getLoginOfGameObjectPlayer(this.gameObject);

        if (this.login == null) {
            Debug.LogError("[CLIENT INFO] start: can't find login");
            return;
        }
        Debug.Log("[CLIENT INFO] <" + this.login + "> start");
        
        this.name = this.login;
        this.num = -1;
        //Player
        this.activateDirections = new bool[] { false, false};
		this.activateFire = false;
		this.rotateQuater = default(Quaternion);
        //this.gameObjectPlayer = this.gameObject;

        this.hasWishes = false;
        this.state = "clientInfo start";
        this.score = 0;
        this.netWManager.clientInfoIsReady(this.login, this);
        instance = this;
    }

    public void setState(string state) { this.state = state; }
    public string getState() { return this.state; }
    public int getNum() { return this.num; }
    public void setNum(int num) { this.num = num; }
    public string getLogin() { return this.login; }
    public void setScore() { this.score++; }
    public int getScore() { return this.score; }

    void fire()
    {
        Debug.Log("[CLIENT INFO] fire spray");
		
        if (this.login == WorldInfo.login)
            this.hasWishes = true;
    }
	void hit(string ennemy)
	{
		//Only the server can decide who should die or not die
		if (Network.isClient)
			return;
		Debug.Log("HITTED BY AN ENNEMY'S <"+ennemy+"> OBJECT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  => CHANGING WORLD");
		
		
		//Sending to client
		NetworkManager.getInstance().forceSendWishes();
		NetworkManager.getInstance().updateScore(ennemy);
	}
    void deplacement(int i, bool b)
    {
        this.activateDirections[i] = b;
        this.hasWishes = true;
    }

    void Update()
    {
        if (WorldInfo.state != "PLAY")
            return;

        if (this.hasWishes) {
            //Debug.Log("[CLIENT INFO] Update: <"+this.login+"> have some wishes");
            this.sendWishesToServer(false);
            this.hasWishes = false;
        }
    }
    void FixedUpdate()
    {
        if (WorldInfo.state != "PLAY")
            return;
        int count = this.activateDirections.Length;
		//Direction
        for (int i = 0; i < count; ++i) {
            if (this.activateDirections[i]) {
                this.transform.forward = ClientInfo.directions[i];
                this.transform.Translate((Vector3.forward * SPEED).normalized * Time.deltaTime);
            }
        }
		//Rotation
		//this.transform.rotation = this.rotateQuater; => go to recept wishes

		//Fire
		if (this.activateFire) {
			fire();
		}
    }

    public void sendWishesToServer(bool state)
    {
        //Debug.Log("[CLIENT INFO] sendWishesToServer <" + this.login + ">");
        clientInformationsSerializable sI = new clientInformationsSerializable();
        sI.activateDirections = this.activateDirections;
		sI.activateFire = this.activateFire;
		sI.rotateQuater = this.rotateQuater;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, sI);

        this.netWManager.sendWishes(this.login, ms.ToArray(), state, 0);
    }

    public void receptWishesFromServer(byte[] wishes)
    {
        //Debug.Log("[CLIENT INFO] <"+this.login+"> receptWishesFromServer");
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(wishes);
        clientInformationsSerializable newSI = (clientInformationsSerializable)bf.Deserialize(ms);
        
        //Player
        this.activateDirections = newSI.activateDirections;
		this.activateFire = newSI.activateFire;
		this.rotateQuater = newSI.rotateQuater;
    }
}
