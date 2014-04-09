using UnityEngine;
using System.Collections;

public class NavManagerScript : MonoBehaviour {

    public NavMeshAgent navAgent;
    public GameObject target;

	// Use this for initialization
	void Start () {
        navAgent.SetDestination(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
