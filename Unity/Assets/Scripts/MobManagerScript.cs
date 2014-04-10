using UnityEngine;
using System.Collections;

public class MobManagerScript : MonoBehaviour {

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
        if (Time.time >= _nextMonsterPopTime)
        {
            _nextMonsterPopTime = Time.time + PopMonsterInterval;
            int spawnP = Random.Range(0, SpawnPoints.Length);
            SpawnMonster(SpawnPoints[spawnP]);
        }
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
}
