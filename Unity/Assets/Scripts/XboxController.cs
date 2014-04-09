using UnityEngine;
using System.Collections;

public class XboxController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            Debug.Log("trigger detected");
    }
}
