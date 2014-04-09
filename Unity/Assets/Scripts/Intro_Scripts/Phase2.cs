using UnityEngine;
using System.Collections;

public class Phase2 : MonoBehaviour
{
    float timer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 11.0f)
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
