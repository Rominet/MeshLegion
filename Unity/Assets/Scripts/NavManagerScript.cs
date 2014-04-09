using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class NavManagerScript : MonoBehaviour {

    [SerializeField]
    private NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent
    {
        get { return _navAgent; }
        set { _navAgent = value; }
    }

    [SerializeField]
    private float _defaultRefreshPathTimer = 1.5f;
    public float DefaultRefreshPathTimer
    {
        get { return _defaultRefreshPathTimer; }
        set { _defaultRefreshPathTimer = value; }
    }

    [SerializeField]
    private float _defaultCloseDistance = 5.0f;
    public float DefaultCloseDistance
    {
        get { return _defaultCloseDistance; }
        set { _defaultCloseDistance = value; }
    }

    private float _nextPathUpdateTime = 0.0f;
    private Transform _targetTransform;

    [SerializeField]
    private GameObject _target;
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

	// Use this for initialization
	void Start () {
        _targetTransform = Target.transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= _nextPathUpdateTime)
        {
            float remainingDist = NavAgent.remainingDistance;
            if (remainingDist >= DefaultCloseDistance)
                _nextPathUpdateTime = Time.time + DefaultRefreshPathTimer;
            else
            {
                float addTime = remainingDist * DefaultRefreshPathTimer / DefaultCloseDistance;
                _nextPathUpdateTime = Time.time + addTime;
            }
            NavAgent.SetDestination(_targetTransform.position);
        }
	}
}
