using UnityEngine;
using System.Collections;

public class Phase1 : MonoBehaviour
{
    float timer;
    
    [SerializeField]
    GameObject phase2;

    // Use this for initialization
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 12.0f)
        {
            this.gameObject.SetActive(false);
            phase2.SetActive(true);
        }
    }
}
