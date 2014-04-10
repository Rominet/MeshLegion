using UnityEngine;
using System.Collections;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class fairyDustInformationsSerializable
{
	public float posX;
	public float posY;
	public float posZ;
	public bool click;
}

public class FairyDustScript : MonoBehaviour, NetworkInterface {

    [SerializeField]
    private ParticleSystem _partSystem;
    public ParticleSystem PartSystem
    {
        get { return _partSystem; }
        set { _partSystem = value; }
    }

    [SerializeField]
    private LayerMask _faeryMask;
    public LayerMask FaeryMask
    {
        get { return _faeryMask; }
        set { _faeryMask = value; }
    }

    public GameObject Sphere;
	private Vector3 pos;
	private bool hasWishes;
    public float _defaultParticleEmissionRate;

    private enum ClickState
    {
        IDLE, CLICKING, RELEASING
    }

    private ClickState leftClick = ClickState.IDLE;

	// Use this for initialization
	void Start () {
        _defaultParticleEmissionRate = PartSystem.emissionRate;
		hasWishes = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            leftClick = ClickState.CLICKING;
            PartSystem.emissionRate = _defaultParticleEmissionRate;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftClick = ClickState.RELEASING;
            PartSystem.emissionRate = 0;
        }
		if (hasWishes) {
			this.sendWishesToServer(false);
			this.hasWishes = false;
		}
	}

    void FixedUpdate()
    {
        if (leftClick == ClickState.CLICKING) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast (ray, out hit, 100, FaeryMask)) {
						Sphere.transform.position = hit.point;
						this.pos = hit.point;
						hasWishes = true;
				}
		} else if (leftClick == ClickState.RELEASING) {
			hasWishes = true;
            leftClick = ClickState.IDLE;
		}
    }
	public void sendWishesToServer(bool state)
	{
		//Debug.Log("[FAIRY DUST] sendWishesToServer <" + WorldInfo.login + ">");
		fairyDustInformationsSerializable sI = new fairyDustInformationsSerializable();
		sI.posX = this.pos.x;
		sI.posY = this.pos.y;
		sI.posZ = this.pos.z;
		sI.click = (leftClick == ClickState.CLICKING);
		
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream();
		bf.Serialize(ms, sI);
		
		NetworkManager.getInstance().sendWishes(WorldInfo.login, ms.ToArray(), state, 1);
	}
	
	public void receptWishesFromServer(byte[] wishes)
	{
		//Debug.Log("[FAIRY DUST] <"+WorldInfo.login+"> receptWishesFromServer");
		BinaryFormatter bf = new BinaryFormatter();
		MemoryStream ms = new MemoryStream(wishes);
		fairyDustInformationsSerializable newSI = (fairyDustInformationsSerializable)bf.Deserialize(ms);
		if (newSI.click) {
			//Debug.Log ("[FAIRY DUST] <" + WorldInfo.login + "> receptWishesFromServer tru");
			PartSystem.emissionRate = _defaultParticleEmissionRate;
			Sphere.transform.position = new Vector3 (newSI.posX, newSI.posY, newSI.posZ);
		} else {
			//Debug.Log("[FAIRY DUST] <"+WorldInfo.login+"> receptWishesFromServer false");
			PartSystem.emissionRate = 0;
		}
	}
}
