using UnityEngine;
using System.Collections;

public class HoveringButtons : MonoBehaviour
{
    [SerializeField]
    Texture[] textures;

    [SerializeField]
    GameObject child;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseEnter()
    {
        child.renderer.material.SetTexture(0, textures[1]); 
    }

    /// <summary>
    /// 
    /// </summary>
    void OnMouseExit()
    {
        child.renderer.material.SetTexture(0, textures[0]); 
    }
}
