using UnityEngine;
using System.Collections;
using Leap;

public class LeapContoller : MonoBehaviour
{
    Controller controller;
    InteractionBox _iBox;
    bool _bitchHasShot;
    float coolDownValue;
    float currCD;
    bool cooldowner;
    double yAvgPos;

    void Start()
    {
        controller = new Controller();
        _bitchHasShot = false;
        coolDownValue = 0.5f;
        currCD = 0.0f;
        cooldowner = false;

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

        if(yAvgPos < 40.0f && !_bitchHasShot && currCD == 0.0f)
        {
            Debug.Log("log that");
            _bitchHasShot = true;
            currCD = coolDownValue;
        }
        else if (yAvgPos > 40.0f && _bitchHasShot && currCD == 0.0f)
        {
            _bitchHasShot = false;
        }

        //if(!_bitchHasShot && frame.Pointables.Count == 0)
        //{
        //    _bitchHasShot = true;
        //    currCD = coolDownValue;
        //    Debug.Log("log that bitch");
        //}
        //else if(_bitchHasShot && frame.Pointables.Count != 0 && currCD == 0.0f)
        //{
        //    _bitchHasShot = false;
        //}

        if (currCD != 0.0f)
        {
            currCD -= Time.deltaTime;
            if (currCD < 0.0f)
                currCD = 0.0f;
        }
    }
}