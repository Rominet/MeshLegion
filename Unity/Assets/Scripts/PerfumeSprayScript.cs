using UnityEngine;
using System.Collections;
using Leap;

public class PerfumeSprayScript : MonoBehaviour
{

    [SerializeField]
    private Transform perfumeSpray;
    public Transform PerfumeSpray
    {
        get { return perfumeSpray; }
        set { perfumeSpray = value; }
    }

    [SerializeField]
    private ParticleSystem _partSystem;
    public ParticleSystem PartSystem
    {
        get { return _partSystem; }
        set { _partSystem = value; }
    }

    Controller _controller;
    bool _princessHasShot;
    float coolDownValue;
    float currCD;
    double yAvgPos;
    ClientInfo clientInfo;

    // Use this for initialization
    void Start()
    {
        _controller = new Controller();
        _princessHasShot = false;
        coolDownValue = 1.0f;
        currCD = 0.0f;
        this.clientInfo = this.transform.parent.GetComponent<ClientInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Frame frame = _controller.Frame();

        Vector avgPos = Vector.Zero;
        foreach (Pointable pointable in frame.Pointables)
        {
            avgPos += pointable.TipPosition;
        }
        avgPos /= frame.Pointables.Count;
        if (avgPos.y != float.NaN)
        {
            yAvgPos = avgPos.y;
        }

        if (yAvgPos < 40.0f && !_princessHasShot && currCD == 0.0f)
        {
            ApplySpray();
            _princessHasShot = true;
            currCD = coolDownValue;
            this.gameObject.audio.Play();
        }
        else if (yAvgPos > 40.0f && _princessHasShot && currCD == 0.0f)
        {
            _princessHasShot = false;
        }

        if (currCD != 0.0f)
        {
            currCD -= Time.deltaTime;
            if (currCD < 0.0f)
                currCD = 0.0f;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            this.clientInfo.fire();
            ApplySpray();
        }
    }

    public void ApplySpray()
    {
        PerfumeSpray.rotation = transform.rotation;
        PerfumeSpray.position = transform.position;
        PartSystem.Play();
    }
}
