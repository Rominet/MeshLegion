using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Application.LoadLevel("Victory");
        }
    }

}
