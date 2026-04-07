using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public InputActionReference sprintAction;

    public AudioClip footSteps;
    private AudioSource footstepSource;

    public AudioClip sprintSound;
    private AudioSource sprintSource;

    private Vector2 movementInput;
    private Rigidbody rb;

    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    private float jumpForce = 5f;

    private bool isSprinting;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        

        footstepSource = gameObject.AddComponent<AudioSource>();
        sprintSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footSteps;
        sprintSource.clip = sprintSound;

        footstepSource.loop = true;
        footstepSource.playOnAwake = false;

        sprintSource.loop = true;
        sprintSource.playOnAwake = false;
        sprintSource.volume = 0.9f;

        footstepSource.spatialBlend = 1f;
        footstepSource.volume = 0.9f;
        footstepSource.pitch = 1f;

        footstepSource.minDistance = 1f;
        footstepSource.maxDistance = 15f;

    }

    void Update()
    {
        bool isMoving = rb.linearVelocity.magnitude > 0.1f;
        isSprinting = sprintAction.action.IsPressed();

        if (isGrounded && isMoving)
        {
            if (isSprinting)
            {
                if (footstepSource.isPlaying) footstepSource.Stop();

                if (!sprintSource.isPlaying)
                {
                    Debug.Log("Playing sprint sound");
                    sprintSource.Play();
                }
            }
            else
            { 
                if (sprintSource.isPlaying) sprintSource.Stop();

                if (!footstepSource.isPlaying)
                {
                    Debug.Log("Playing footstep sound");
                    footstepSource.Play();
                }
            }
        }
        else
        {
            if (footstepSource.isPlaying) footstepSource.Stop();
            if (sprintSource.isPlaying) sprintSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        checkPlayerIsGrounded();
        MovePlayer();
    }

    public void OnMovement(InputValue data)
    {
        movementInput = data.Get<Vector2>();
    }

    public void OnJump(InputValue data)
    {
        if (isGrounded)
        {

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    public void MovePlayer()
    {
        Vector3 direction = transform.right * movementInput.x  + transform.forward * movementInput.y;
        //Debug.Log($"Direction: {direction}");

        if (isSprinting)
        {
            movementSpeed = 7.5f;
        } else
        {
            movementSpeed = 5f;
        }
        rb.linearVelocity = new Vector3(direction.x * movementSpeed, rb.linearVelocity.y, direction.z * movementSpeed);
    }

    private void checkPlayerIsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Debug.Log($"Is Grounded: {isGrounded}");
    }
}
