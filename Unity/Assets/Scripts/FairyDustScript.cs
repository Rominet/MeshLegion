using UnityEngine;
using System.Collections;

public class FairyDustScript : MonoBehaviour {

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

    [SerializeField]
    private Transform _aggroSpray;
    public Transform AggroSpray
    {
        get { return _aggroSpray; }
        set { _aggroSpray = value; }
    }

    public GameObject Sphere;
    private float _defaultParticleEmissionRate;

    private enum ClickState
    {
        IDLE, CLICKING, RELEASING
    }

    private ClickState leftClick = ClickState.IDLE;
    private ClickState rightClick = ClickState.IDLE;

    private AggroSprayScript _aggroScript;

    private float _nextAggroSprayTime;

	// Use this for initialization
	void Start () {
        _defaultParticleEmissionRate = PartSystem.emissionRate;
        _aggroScript = AggroSpray.GetComponent<AggroSprayScript>();
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
        if (Input.GetMouseButtonDown(1))
        {
            rightClick = ClickState.CLICKING;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightClick = ClickState.IDLE;
        }
	}

    void FixedUpdate()
    {
        if (leftClick == ClickState.CLICKING)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100,FaeryMask))
            {
                    Sphere.transform.position = hit.point;
            }
        }
        if (rightClick == ClickState.CLICKING)
        {
            if (Time.time >= _nextAggroSprayTime)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100,FaeryMask))
                {
                    AggroSpray.position = hit.point;
                    _aggroScript.StartAggro();
                    _nextAggroSprayTime = Time.time + _aggroScript.AggroDuration * 2;
                }
            }
        }
    }

}
