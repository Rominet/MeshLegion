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
<<<<<<< HEAD
        //int positionX = (int)(this.transform.position.x);
        //Borders[(int)(_BLabel.LEFT)].transform.position = new Vector3(-positionX, 20.5f, 0.0f);
        //BoxCollider col = (BoxCollider)Borders[(int)(_BLabel.LEFT)].collider;
        //col.size = new Vector3(1, 1, ArenaSize[1]);
=======
        float positionX = this.transform.position.x;
        Borders[(int)(_BLabel.LEFT)].transform.position = new Vector3(-positionX, 0.5f, 0.0f);
        BoxCollider col = (BoxCollider)Borders[(int)(_BLabel.LEFT)].collider;
        col.size = new Vector3(1, 1, ArenaSize[1]);
>>>>>>> acc38ca0e903db199fa442ff52de6474b7e84193


        //Borders[(int)(_BLabel.RIGHT)].transform.position = new Vector3(positionX, 20.5f, 0.0f);
        //col = (BoxCollider)Borders[(int)(_BLabel.RIGHT)].collider;
        //col.size = new Vector3(1, 1, ArenaSize[1]);

<<<<<<< HEAD
        //int positionY = (int)(this.transform.position.y);
        //Borders[(int)(_BLabel.FRONT)].transform.position = new Vector3(0.0f, 20.5f, ArenaSize[1] / 2);
        //col = (BoxCollider)Borders[(int)(_BLabel.FRONT)].collider;
        //col.size = new Vector3(ArenaSize[0], 1, 1);
=======
        float positionY = this.transform.position.y;
        Borders[(int)(_BLabel.FRONT)].transform.position = new Vector3(0.0f, 0.5f, positionY);
        col = (BoxCollider)Borders[(int)(_BLabel.FRONT)].collider;
        col.size = new Vector3(ArenaSize[0], 1, 1);
>>>>>>> acc38ca0e903db199fa442ff52de6474b7e84193


        //Borders[(int)(_BLabel.BACK)].transform.position = new Vector3(0.0f, 20.5f, -ArenaSize[1] / 2);
        //col = (BoxCollider)Borders[(int)(_BLabel.BACK)].collider;
        //col.size = new Vector3(ArenaSize[0], 1, 1);

    }
}
