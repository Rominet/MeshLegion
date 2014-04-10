/* --------------------------Header-------------------------------------
 * File : CameraBorderScript.cs
 * Description : Script that handles the borders of an arena to stop camera moving.
 * Version : 1.0.0
 * Created Date : 05/11/2013 11:46:46
 * Created by : Jonathan Bihet
 * Modification Date : 05/11/2013 18:08:33
 * Modified by : Jonathan Bihet
 * ------------------------------------------------------------------------ */


using UnityEngine;
using System.Collections;

public class CameraBorderScript : MonoBehaviour
{
    private enum _BLabel { LEFT = 0, RIGHT, FRONT, BACK };

    [SerializeField]
    private GameObject[] _borders;
    public GameObject[] Borders
    {
        get { return _borders; }
        set { _borders = value; }
    }

    [SerializeField]
    private Vector2 _arenaSize;
    public Vector2 ArenaSize
    {
        get { return _arenaSize; }
        set { _arenaSize = value; }
    }

    void Start()
    {


    }
}
