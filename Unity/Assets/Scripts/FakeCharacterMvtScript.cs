using UnityEngine;
using System.Collections;

public class FakeCharacterMvtScript : MonoBehaviour {

    [SerializeField]
    private NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent
    {
        get { return _navAgent; }
        set { _navAgent = value; }
    }

    [SerializeField]
    private GameObject _target;
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

	// Use this for initialization
	void Start () {
        NavAgent.SetDestination(Target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
