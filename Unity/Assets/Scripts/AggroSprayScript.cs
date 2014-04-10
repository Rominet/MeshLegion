using UnityEngine;
using System.Collections;

public class AggroSprayScript : MonoBehaviour {

    [SerializeField]
    private float _aggroRadius = 3;
    public float AggroRadius
    {
        get { return _aggroRadius; }
        set { _aggroRadius = value; }
    }

    [SerializeField]
    private float _aggroDuration;
    public float AggroDuration
    {
        get { return _aggroDuration; }
        set { _aggroDuration = value; }
    }

    [SerializeField]
    private ParticleSystem _partSystem;
    public ParticleSystem PartSystem
    {
        get { return _partSystem; }
        set { _partSystem = value; }
    }

    private Collider[] _closeMonsters;
    private float _disableTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= _disableTime)
        {
            StopAggro();
        }
	}

    public void StopAggro()
    {
        PartSystem.Stop();
    }

    public void StartAggro()
    {
        _disableTime = Time.time + AggroDuration * 2;
        _closeMonsters = Physics.OverlapSphere(transform.position, AggroRadius);
        foreach (var col in _closeMonsters)
        {
            MonsterScript ms = col.GetComponent<MonsterScript>();
            if (ms != null)
                ms.Aggro(transform.position, AggroDuration);
        }

        PartSystem.Play();
    }
}
