using UnityEngine;
using System.Collections;
using Leap;

public class LeapContoller : MonoBehaviour
{
    Controller controller;
    InteractionBox _iBox;
    bool _princessHasShot;
    float coolDownValue;
    float currCD;
    double yAvgPos;

    void Start()
    {
        controller = new Controller();
        _princessHasShot = false;
        coolDownValue = 1.0f;
        currCD = 0.0f;

    }

    void Update()
    {
        Frame frame = controller.Frame();

        Vector avgPos = Vector.Zero;
        foreach(Pointable pointable in frame.Pointables)
        {
            avgPos += pointable.TipPosition;
        }
        avgPos /= frame.Pointables.Count;
        if(avgPos.y != float.NaN)
        {
            yAvgPos = avgPos.y;
        }

        if(yAvgPos < 40.0f && !_princessHasShot && currCD == 0.0f)
        {
            Debug.Log("log that");
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
    }
}