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

    PlayerStats Stats;

    private void Awake()
    {
      
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

   
    void Update()
    {
        Move();
        Gamepad[] pads = Gamepad.all.ToArray();
        if (pads.Length < 1)
        {
            Debug.LogError("Connect A Controller!!!");
            return;
        }
       
        pDevicePad = pads[0];
        if (pDevicePad.rightTrigger.wasPressedThisFrame)
        {
           Attack();
        }
        if (pDevicePad.rightShoulder.wasPressedThisFrame)
        {
            Switch();
        }
        if (pDevicePad.buttonNorth.wasPressedThisFrame)
        {
           if (isGrounded)
           {
              Jump();
           }
        }
        if (pDevicePad.buttonEast.wasPressedThisFrame)
        {
            Run();
        }
        if (pDevicePad.buttonWest.wasPressedThisFrame)
        {
            Reload();
        }
        if (pDevicePad.buttonSouth.wasPressedThisFrame)
        {
            Interact();
        }
    }


    public void Move()
    {
        Vector3 movement = new Vector3(Moveet.x, 0, Moveet.y) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
    private void OnMove(InputValue value)
    {
        Moveet = value.Get<Vector2>();
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

    public void Run()
    {
        Debug.Log("Run");
        //Switch to Run
    }

    public void Reload()
    {
        Debug.Log("Reload");
        //Reload Weapons
    }
    public void Interact()
    {
        Debug.Log("Interact");
        //Interact??
    }
    public void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

}

