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

    public GameObject Sphere;
    public float _defaultParticleEmissionRate;

    private enum ClickState
    {
        IDLE, CLICKING, RELEASING
    }

    private ClickState leftClick = ClickState.IDLE;

	// Use this for initialization
	void Start () {
        _defaultParticleEmissionRate = PartSystem.emissionRate;
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
    }

}
