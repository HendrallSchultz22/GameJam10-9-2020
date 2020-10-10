using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;
    public Vector2 Cammove;
    Gamepad PDevice;
    void Start()
    {

    }


    void LateUpdate()
    {
        CameraMove();
    }

    void CameraMove()
    {
        Gamepad[] pads = Gamepad.all.ToArray();
        if (pads.Length < 1)
        {
            Debug.LogError("Connect a Controller");
            return;
        }
        else
        {
            PDevice = pads[0];
            mouseX += PDevice.rightStick.x.ReadValue() * RotationSpeed;
            mouseY -= PDevice.rightStick.y.ReadValue() * RotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -35, 60);

            transform.LookAt(Target);

            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            Player.rotation = Quaternion.Euler(0, mouseX, 0);
        }

    }

    public void OnCameraMove(InputValue value)
    {
        Cammove = value.Get<Vector2>();
    }
}
