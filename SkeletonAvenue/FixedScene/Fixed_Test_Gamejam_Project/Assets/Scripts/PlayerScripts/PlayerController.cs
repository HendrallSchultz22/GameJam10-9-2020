using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce = 300f;
    public bool isGrounded;
    public bool isRunning;
    [SerializeField] public float movementSpeed = 25f;
    [SerializeField] public float backSpeed = 20f;

    Animator anim;

    Vector2 Moveet;
    Vector3 moveDirection;
    Vector3 movement;

    public InputDevice pDevice;

    public Gamepad pDevicePad;

    

    public PlayerStats player;
    

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
       
        
        if (pDevicePad.buttonNorth.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                Jump();
            }
        }
        if (pDevicePad.buttonEast.isPressed)
        {
            isRunning = true;
            Run();
        }
        if (!pDevicePad.buttonEast.isPressed)
        {
            isRunning = false;
            if (!isRunning)
            {
                movementSpeed = 25.0f;
            }
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
   
    public void Run()
    {
        Debug.Log("Run");
        if (player.playerEndurance > 0 && isRunning)
        {
            movementSpeed = 45.0f;

            player.playerEndurance -= 0.8f;
        }
        //Switch to Run
    }

    public void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        isGrounded = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Domain" || other.gameObject.tag == "NotDomain")
        {
            isGrounded = true;
        }
    }
}