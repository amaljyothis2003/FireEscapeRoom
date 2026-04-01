using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpForce = 6f;
    public float groundCheckDistance = 1.1f;

    [Header("Mouse Look")]
    public Transform cameraTransform;
    public float sensitivity = 3f;
    public float smoothing = 5f;

    [Header("Footsteps")]
    public AudioSource footstepAudio;
    public AudioClip[] footstepClips;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public float footstepInterval = 0.45f;

    Rigidbody rb;
    bool isGrounded;
    bool wasGrounded;
    float xRot;
    float stepTimer;

    Vector2 smoothedVelocity;
    Vector2 currentMouseDelta;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;    // Prevent player from tilting

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseLook();
        CheckGround();
        HandleFootsteps();
        HandleLanding();
    }

    void FixedUpdate()
    {
        MovePlayer();
        Jump();
    }

    // -----------------------
    //  MOVEMENT
    // -----------------------
    void MovePlayer()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = (transform.right * h + transform.forward * v).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 targetVelocity = dir * speed;
        Vector3 currentVelocity = rb.linearVelocity;

        Vector3 velocityChange = new Vector3(
            targetVelocity.x - currentVelocity.x,
            0, // don't affect Y for gravity/jumps
            targetVelocity.z - currentVelocity.z
        );

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // -----------------------
    //  JUMP
    // -----------------------
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            PlayJumpSound();
        }
    }

    // -----------------------
    //  MOUSE LOOK (smooth)
    // -----------------------
    void MouseLook()
    {
        Vector2 md = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        md *= sensitivity;

        currentMouseDelta = Vector2.Lerp(currentMouseDelta, md, 1f / smoothing);

        transform.Rotate(Vector3.up * currentMouseDelta.x);

        xRot -= currentMouseDelta.y;
        xRot = Mathf.Clamp(xRot, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    // -----------------------
    //  FOOTSTEPS
    // -----------------------
    void HandleFootsteps()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        bool isMoving = (h != 0 || v != 0);

        if (isGrounded && isMoving)
        {
            stepTimer += Time.deltaTime;

            float interval = Input.GetKey(KeyCode.LeftShift) ? footstepInterval * 0.6f : footstepInterval;

            if (stepTimer >= interval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;  // reset when not moving
        }
    }

    void PlayFootstepSound()
    {
        if (footstepAudio != null)
        {
            if (footstepClips != null && footstepClips.Length > 0)
            {
                AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                footstepAudio.PlayOneShot(clip);
            }
            else
            {
                footstepAudio.Play();
            }
        }
    }

    void PlayJumpSound()
    {
        if (footstepAudio != null && jumpSound != null)
        {
            footstepAudio.PlayOneShot(jumpSound);
        }
    }

    void HandleLanding()
    {
        if (isGrounded && !wasGrounded)
        {
            // Just landed
            if (footstepAudio != null && landSound != null)
            {
                footstepAudio.PlayOneShot(landSound);
            }
        }
        wasGrounded = isGrounded;
    }

    // -----------------------
    //  GROUND CHECK
    // -----------------------
    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}