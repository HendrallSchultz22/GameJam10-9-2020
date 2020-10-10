using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce = 300f;
    public bool isGrounded;
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float backSpeed = 5f;

    Vector2 Moveet;
    Vector3 moveDirection;
    Vector3 movement;

    public InputDevice pDevice;
   
    public Gamepad pDevicePad;

  
   
    private void Awake()
    {
      
    }

    void Start()
    {
        Gamepad[] pads = Gamepad.all.ToArray();

        if (pads.Length < 1)
        {
            Debug.LogError("Connect a Controller");
            return;
        }

        pDevice = pads[0].device;

        pDevicePad = pads[0];
       
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
  
    }

   
    void Update()
    {
        if (pDevicePad.buttonWest.wasPressedThisFrame)
        {
            Attack();
        }
        if (pDevicePad.buttonNorth.wasPressedThisFrame)
        {
            Switch();
        }
        if (pDevicePad.buttonSouth.wasPressedThisFrame)
        {
            Reload();
        }
        if (pDevicePad.buttonEast.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                Jump();
            }
            
        }


        //horizontal
        if (pDevicePad.leftStick.x.ReadValue() > 0)
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        //vertical 
        if (pDevicePad.leftStick.y.ReadValue() > 0)
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        //horizontal 
        if (pDevicePad.leftStick.x.ReadValue() < 0)
        {
           transform.position += Vector3.left * backSpeed * Time.deltaTime;
        }
        //vertical 
        if (pDevicePad.leftStick.y.ReadValue() < 0)
        {
           transform.position += Vector3.back * movementSpeed * Time.deltaTime;
        }

    }

    

    public void Attack()
    {
        Debug.Log("Attack");
        //Shoot bullet
    }
    
    public void Switch()
    {
        Debug.Log("Switch");
        //Switch Weapons
    }

    public void Reload()
    {
        Debug.Log("Reload");
        //Reload Weapons
    }

    public void Jump()
    {
        Debug.Log("Jump");
        //....Jump?
    }

    private void OnMove(InputValue value)
    {
        Moveet = value.Get<Vector2>();
    }
}

