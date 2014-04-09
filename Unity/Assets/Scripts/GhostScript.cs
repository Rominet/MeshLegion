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

    private bool _allowedToMove = true;
    public bool AllowedToMove
    {
        get { return _allowedToMove; }
        set { _allowedToMove = value; }
    }

    [SerializeField]
    private float _defaultRefreshPathTimer = 1.5f;
    public float DefaultRefreshPathTimer
    {
        get { return _defaultRefreshPathTimer; }
        set { _defaultRefreshPathTimer = value; }
    }

    private float _nextPathUpdateTime;

	// Use this for initialization
	void Start () {
        _targetTransform = Target.transform;
        _transform = transform;
        rigidbody.velocity = new Vector3(0, 0, MovementSpeed);
	}

	// Update is called once per frame
	void FixedUpdate () {

        if (Time.time >= _nextPathUpdateTime)
        {
            _nextPathUpdateTime = Time.time + DefaultRefreshPathTimer;
            if (AllowedToMove)
            {
                _transform.LookAt(_targetTransform.position);
                rigidbody.velocity = transform.forward * MovementSpeed;
            }
        }
	}
}
