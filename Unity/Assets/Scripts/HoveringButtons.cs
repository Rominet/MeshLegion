using UnityEngine;
using System.Collections;

public class HoveringButtons : MonoBehaviour
{
    [SerializeField]
    Texture[] textures;

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
        this.gameObject.renderer.material.SetTexture(0, textures[1]); 
    }

    /// <summary>
    /// 
    /// </summary>
    void OnMouseExit()
    {
        this.gameObject.renderer.material.SetTexture(0, textures[0]); 
    }
}
