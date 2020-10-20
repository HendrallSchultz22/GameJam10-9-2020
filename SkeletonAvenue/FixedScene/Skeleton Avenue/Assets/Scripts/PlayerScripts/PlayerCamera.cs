using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float RotationSpeed = 1;
    public Transform Target, Player;
    float Xaxis, Yaxis;
    public Vector2 Cammove;
    Gamepad PDevice;
    [SerializeField] float maxVertClamp;
    [SerializeField] float minVertClamp;
    void Start()
    {


    }


    void Update()
    {
        Gamepad[] pads = Gamepad.all.ToArray();
        if (pads.Length < 1)
        {
            Debug.LogError("Connect A Controller!!!");
            return;
        }
        PDevice = pads[0];
        CameraMove();
    }


    void CameraMove()
    {
        
        Xaxis += PDevice.rightStick.x.ReadValue() * RotationSpeed;
        Yaxis -= PDevice.rightStick.y.ReadValue() * RotationSpeed;

        Yaxis = Mathf.Clamp(Yaxis, minVertClamp, maxVertClamp);

        transform.rotation = Quaternion.Euler(Yaxis, Xaxis, 0);
        Player.rotation = Quaternion.Euler(0, Xaxis, 0);

    }

    public void OnCameraMove(InputValue value)
    {
        Cammove = value.Get<Vector2>();
    }

}