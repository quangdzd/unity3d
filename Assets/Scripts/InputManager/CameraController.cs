using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour , IInputAction
{
    private Vector3 cam_Pos;
    private Quaternion cam_Rot;

    [SerializeField] private float m_Speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float scrollSpeed;

    void Awake()
    {
        cam_Pos = transform.position;
        cam_Rot = transform.rotation;
    }
    private void Start()
    {
        RegisterListener();
    }
    public void ResetCamera()
    {
        transform.position = cam_Pos;
        transform.rotation = cam_Rot;
    }



    public void RegisterListener()
    {
        InputManager.Instance.RegisterAction(OnUpButtonDown, MoveUp);
        InputManager.Instance.RegisterAction(OnDownButtonDown, MoveDown);
        InputManager.Instance.RegisterAction(OnRightButtonDown, MoveRight);
        InputManager.Instance.RegisterAction(OnLeftButtonDown, MoveLeft);
        InputManager.Instance.RegisterAction(OnMouse1Button, RotateCamera);
        InputManager.Instance.RegisterAction(OnScroll, ScrollCamera);
    }

    public void UnregisterListener()
    {
        throw new System.NotImplementedException();
    }
    public bool OnLeftButtonDown()
    {
        return Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    }

    public bool OnRightButtonDown()
    {
        return Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public bool OnDownButtonDown()
    {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    public bool OnUpButtonDown()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    }
    public bool OnScroll()
    {
        return Input.GetAxis("Mouse ScrollWheel") != 0;
    }
    public bool OnMouse1Button()
    {
        return Input.GetMouseButton(1);
    }

    public void MoveUp()
    {
        gameObject.transform.position +=  m_Speed * Time.unscaledDeltaTime*gameObject.transform.forward;
    }
    public void MoveDown()
    {
        gameObject.transform.position +=  -m_Speed * Time.unscaledDeltaTime*gameObject.transform.forward;

    }
    public void MoveLeft()
    {
        gameObject.transform.position +=  -m_Speed * Time.unscaledDeltaTime*gameObject.transform.right;

    }
    public void MoveRight()
    {
        gameObject.transform.position +=  m_Speed * Time.unscaledDeltaTime*gameObject.transform.right;

    }
    public void RotateCamera()
    {
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");

        gameObject.transform.Rotate(Vector3.up , mouse_x * rotateSpeed  , Space.World);

        gameObject.transform.Rotate(Vector3.right ,-mouse_y * rotateSpeed , Space.Self);


    }
    public void ScrollCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward *scroll * scrollSpeed * Time.unscaledDeltaTime;

    }

}
