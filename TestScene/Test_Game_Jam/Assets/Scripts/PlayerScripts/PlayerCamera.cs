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
    void Start()
    {
     
       
    }


    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    { 
        Gamepad[] pads = Gamepad.all.ToArray();
        if (pads.Length < 1)
        {
            Debug.LogError("Connect A Controller!!!");
            return;
        }
        PDevice = pads[0];
        Xaxis += PDevice.rightStick.x.ReadValue() * RotationSpeed;
        Yaxis -= PDevice.rightStick.y.ReadValue() * RotationSpeed;
       

        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(Yaxis, Xaxis, 0);
        Player.rotation = Quaternion.Euler(0, Xaxis, 0);
      
    }

    public void OnCameraMove(InputValue value)
    {
        Cammove = value.Get<Vector2>();
    }
   
}
