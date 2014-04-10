using UnityEngine;
using System.Collections;

public class GhostScript : MonoBehaviour {

    [SerializeField]
    private float _movementSpeed = 5;
    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

    [SerializeField]
    private GameObject _target;
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    private Transform _targetTransform;
    private Transform _transform;

    private bool _allowedToMove = false;
    public bool AllowedToMove
    {
        get { return _allowedToMove; }
        set { _allowedToMove = value; }
    }

    [SerializeField]
    private Vector3 _targetPos;
    public Vector3 TargetPos
    {
      get { return _targetPos; }
      set { _targetPos = value; }
    }

    [SerializeField]
    private float _defaultRefreshPathTimer = 1.5f;
    public float DefaultRefreshPathTimer
    {
        get { return _defaultRefreshPathTimer; }
        set { _defaultRefreshPathTimer = value; }
    }

    [SerializeField]
    private float _nextPathUpdateTime;
    public float NextPathUpdateTime
    {
      get { return _nextPathUpdateTime; }
      set { _nextPathUpdateTime = value; }
    }

	// Use this for initialization
	void Start () {
        if (Target != null)
            _targetTransform = Target.transform;
        else
            _targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _transform = transform;
	}

	// Update is called once per frame
	void FixedUpdate () {

        if (Time.time >= NextPathUpdateTime)
        {
            NextPathUpdateTime = Time.time + DefaultRefreshPathTimer;
            if (_targetTransform != null)
                TargetPos = _targetTransform.position;
            if (AllowedToMove)
            {
                _transform.LookAt(TargetPos);
                rigidbody.velocity = transform.forward * MovementSpeed;
            }
        }
	}
}
