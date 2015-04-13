using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class clientInformationsSerializable
{
	public float posX;
	public float posY;
	public float posZ;

	public bool activateFire;
	public float rotateQuaterX;
	public float rotateQuaterY;
	public float rotateQuaterZ;
	public float rotateQuaterW;
}


public class ClientInfo : MonoBehaviour, NetworkInterface
{
	//Singleton
	private static ClientInfo instance;
	public static ClientInfo getInstance() { return instance; }

	private static readonly Vector3[] directions = new[] { Vector3.forward, Vector3.back};
    private static readonly float SPEED = 4.0f;

    private NetworkManager netWManager;
    private Transform cameraOVR;

	private Quaternion rotateQuater;
	private Vector3 pos;
	private PerfumeSprayScript pss;

    private int num;
    private bool hasWishes;
    private string login;
    private string state;
    private int score;

    //Player
	private bool activateFire;

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
		this.activateFire = false;
        this.cameraOVR = GetComponentInChildren<OVRPlayerController>().transform;
		this.rotateQuater = default(Quaternion);
        this.hasWishes = false;
        this.state = "clientInfo start";
        this.score = 0;

        this.pos = this.transform.position;
        this.rotateQuater = this.cameraOVR.rotation;

        this.netWManager.clientInfoIsReady(this.login, this);
        instance = this;

		this.pss = GetComponentInChildren<PerfumeSprayScript>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            this.pos = this.transform.position;
            this.hasWishes = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            this.rotateQuater = this.cameraOVR.rotation;
            this.hasWishes = true; 
        }
        if (this.hasWishes)
        {
            //Debug.Log("[CLIENT INFO] Update: <"+this.login+"> have some wishes");
            this.sendWishesToServer(false);
            this.hasWishes = false;
        }
    }

    public void rotate(Quaternion q)
    {
        this.rotateQuater = q;
        this.hasWishes = true; 
    }

    public void setState(string state) { this.state = state; }
    public string getState() { return this.state; }
    public int getNum() { return this.num; }
    public void setNum(int num) { this.num = num; }
    public string getLogin() { return this.login; }
    public void setScore() { this.score++; }
    public int getScore() { return this.score; }
	
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
	public void fire()
	{
		//Debug.Log("[CLIENT INFO] fire spray");
		this.activateFire = true;
		this.hasWishes = true;
	}

    public void sendWishesToServer(bool state)
    {
        //Debug.Log("[CLIENT INFO] sendWishesToServer <" + this.login + ">");
        clientInformationsSerializable sI = new clientInformationsSerializable();
        sI.posX = this.pos.x;
		sI.posY = this.pos.y;
		sI.posZ = this.pos.z;
		sI.activateFire = this.activateFire;
		this.activateFire = false;


		sI.rotateQuaterX = this.rotateQuater.x;
		sI.rotateQuaterY = this.rotateQuater.y;
		sI.rotateQuaterZ = this.rotateQuater.z;
		sI.rotateQuaterW = this.rotateQuater.w;

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, sI);

        this.netWManager.sendWishes(this.login, ms.ToArray(), state, 0);
    }

    public void receptWishesFromServer(byte[] wishes)
    {
		if (Network.isClient) {
						return;
				}
        //Debug.Log("[CLIENT INFO] <"+this.login+"> receptWishesFromServer");
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(wishes);
        clientInformationsSerializable newSI = (clientInformationsSerializable)bf.Deserialize(ms);
        
        //Player
		this.transform.position = new Vector3 (newSI.posX, newSI.posY, newSI.posZ);

		if (newSI.activateFire)
		{
            pss.ApplySpray();
		}
		this.cameraOVR.rotation = new Quaternion (newSI.rotateQuaterX, newSI.rotateQuaterY, newSI.rotateQuaterZ, newSI.rotateQuaterW);
    }
}
