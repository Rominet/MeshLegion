using UnityEngine;
using System.Collections;

public class ThirdCameraScript : MonoBehaviour
{
    [SerializeField]
    private float _cameraSpeed;
    public float CameraSpeed
    {
        get { return _cameraSpeed; }
        set { _cameraSpeed = value; }
    }

    private bool _canMoveLeft = true;
    private bool _canMoveRight = true;
    private bool _canMoveFront = true;
    private bool _canMoveBack = true;

    private float _mouseBorderDetect = 30.0f;
    private Vector3 _cameraDirection;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        _cameraDirection = Vector3.zero;

        if (Input.mousePosition.x >= Screen.width - _mouseBorderDetect)
        {
            if (_canMoveRight)
                _cameraDirection.x = 1.0f;
        }
        if (Input.mousePosition.x <= _mouseBorderDetect)
        {
            if (_canMoveLeft)
                _cameraDirection.x = -1.0f;
        }

        if (Input.mousePosition.y >= Screen.height - _mouseBorderDetect)
        {
            if (_canMoveFront)
                _cameraDirection.z = 1.0f;
        }
        if (Input.mousePosition.y <= _mouseBorderDetect)
        {
            if (_canMoveBack)
                _cameraDirection.z = -1.0f;
        }
        transform.Translate(_cameraDirection.normalized * CameraSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("LeftBorder"))
        {
            _canMoveLeft = false;
        }
        if (other.transform.name.Equals("RightBorder"))
        {
            _canMoveRight = false;
        }
        if (other.transform.name.Equals("FrontBorder"))
        {
            _canMoveFront = false;
        }
        if (other.transform.name.Equals("BackBorder"))
        {
            _canMoveBack = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.name.Equals("LeftBorder"))
        {
            _canMoveLeft = true;
        }
        if (other.transform.name.Equals("RightBorder"))
        {
            _canMoveRight = true;
        }
        if (other.transform.name.Equals("FrontBorder"))
        {
            _canMoveFront = true;
        }
        if (other.transform.name.Equals("BackBorder"))
        {
            _canMoveBack = true;
        }
    }

}
