using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MobInfosSerializable
{
    public float posX;
    public float posY;
    public float posZ;
    public float rotateQuaterX;
    public float rotateQuaterY;
    public float rotateQuaterZ;
    public float rotateQuaterW;
}
[System.Serializable]
public class MobManagerInfosSerializable
{
    public List<MobInfosSerializable> listMIS;
}


public class MobManagerScript : MonoBehaviour, NetworkInterface {

    [SerializeField]
    private Transform[] _spawnPoints;
    public Transform[] SpawnPoints
    {
        get { return _spawnPoints; }
        set { _spawnPoints = value; }
    }

    [SerializeField]
    private GameObject[] _mobs;
    public GameObject[] Mobs
    {
        get { return _mobs; }
        set { _mobs = value; }
    }

    [SerializeField]
    private float _popMonsterInterval;
    public float PopMonsterInterval
    {
        get { return _popMonsterInterval; }
        set { _popMonsterInterval = value; }
    }

    private float _nextMonsterPopTime = 0;
    private int _nextMonsterToPop = 0;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Network.isServer)
            return;
        if (Time.time >= _nextMonsterPopTime)
        {
            _nextMonsterPopTime = Time.time + PopMonsterInterval;
            int spawnP = Random.Range(0, SpawnPoints.Length);
            SpawnMonster(SpawnPoints[spawnP]);
        }
	}
    void FixedUpdate()
    {
        if (Network.isServer)
            return;
        sendWishesToServer(false);
    }

    void SpawnMonster(Transform spawnTransform)
    {
        float posX = spawnTransform.position.x;// +Random.Range(-spawnTransform.localScale.x, spawnTransform.localScale.x);
        float posY = 0.25f;
        float posZ = spawnTransform.position.z;// +Random.Range(-spawnTransform.localScale.z, spawnTransform.localScale.z);

        if (_nextMonsterToPop < Mobs.Length)
        {
            Mobs[_nextMonsterToPop].transform.parent = null;
            Mobs[_nextMonsterToPop].GetComponent<MonsterScript>().Spawn(new Vector3(posX, posY, posZ)); 
        }
        _nextMonsterToPop++;
    }

    public void sendWishesToServer(bool state)
    {
        Debug.Log("[MOB MANAGER] sendWishesToServer");
        MobManagerInfosSerializable mMSI = new MobManagerInfosSerializable();
        mMSI.listMIS = new List<MobInfosSerializable>();
        MobInfosSerializable mSI;// = new WorldInfosSerializable();
        //List<transformSerializable> listTS = new List<transformSerializable>();

        Transform mobTransform = null;
        foreach (GameObject mob in Mobs)
        {
            mobTransform = mob.transform;
            mSI = new MobInfosSerializable();
            mSI.posX = mobTransform.position.x;
            mSI.posY = mobTransform.position.y;
            mSI.posZ = mobTransform.position.z;
            mSI.rotateQuaterW = mobTransform.rotation.w;
            mSI.rotateQuaterX = mobTransform.rotation.x;
            mSI.rotateQuaterY = mobTransform.rotation.y;
            mSI.rotateQuaterZ = mobTransform.rotation.z;
            
            mMSI.listMIS.Add(mSI);
        }

        NetworkManager.getInstance().sendWishes(WorldInfo.login, NetworkManager.objectToByte(mMSI), state, 2);
    }

    public void receptWishesFromServer(byte[] wishes)
    {
                Debug.Log("[MOB MANAGER] receptWishesFromServer");
        MobManagerInfosSerializable mMSI = (MobManagerInfosSerializable)NetworkManager.byteToObject(wishes);
        int i = 0;
        foreach (MobInfosSerializable mobInfo in mMSI.listMIS)
        {
            Mobs[i].transform.position = new Vector3(mobInfo.posX,mobInfo.posY,mobInfo.posZ);
            Mobs[i].transform.rotation = new Quaternion(mobInfo.rotateQuaterX, mobInfo.rotateQuaterY, mobInfo.rotateQuaterZ, mobInfo.rotateQuaterW);
            ++i;
        }
    }

}
