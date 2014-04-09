using UnityEngine;
using System.Collections;

public class FairyDustScript : MonoBehaviour {

    [SerializeField]
    private LayerMask _faeryMask;
    public LayerMask FaeryMask
    {
        get { return _faeryMask; }
        set { _faeryMask = value; }
    }

    public GameObject Sphere;
    public Vector3 _defaultParticlePosition;

    private bool clicking = false;

	// Use this for initialization
	void Start () {
        _defaultParticlePosition = Sphere.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            clicking = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            clicking = false;
            Sphere.transform.position = _defaultParticlePosition;
        }
	}

    void FixedUpdate()
    {
        if (clicking)
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
