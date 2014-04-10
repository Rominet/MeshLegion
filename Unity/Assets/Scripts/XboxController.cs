using UnityEngine;
using System.Collections;


public class XboxController : MonoBehaviour
{
    bool _princessHasShot;
    float coolDownValue;
    float currCD;

    void Start()
    {
        _princessHasShot = false;
        coolDownValue = 1.0f;
        currCD = 0.0f;
    }

    void Update()
    {
        float leftTrigger = Input.GetAxis("3rd");

        if(leftTrigger > 0.5f && !_princessHasShot && currCD == 0.0f)
        {
            Debug.Log("log that 1");
            _princessHasShot = true;
            currCD = coolDownValue;
            this.gameObject.audio.Play();
        }
        else if (leftTrigger < 0.5f && _princessHasShot && currCD == 0.0f) 
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
