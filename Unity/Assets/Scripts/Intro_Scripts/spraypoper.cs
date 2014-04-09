using UnityEngine;
using System.Collections;

public class spraypoper : MonoBehaviour
{
    [SerializeField]
    GameObject[] sprayTexture;

    int current;
    float timer;
    float stepper;
    float decrementer;

    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
        current = 0;
        stepper = 0.5f;
        decrementer = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= stepper)
        {  
            if(current < sprayTexture.Length-1)
            {
                sprayTexture[current].SetActive(true);
                current++;
                stepper -= decrementer;
                decrementer += decrementer/2;
            }

            timer = 0.0f;
        }

    }
}
