using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public Transform orientation;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool isWalking = false;
    private bool readyToJump;
    private bool readyToDoubleJump = false;
    public bool doubleJumpActive = false;
    public bool canMove = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Sound")]
    public AudioClip jumpSound;
    public AudioClip[] footSteps;
    private AudioSource audioSource;
    private float originalPitch;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
        originalPitch = audioSource.pitch;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        if (GameManager.instance.CurrentGameState != GameManager.GameState.Lost)
        {
            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

            MyInput();
            SpeedControl();

            // handle drag
            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
                rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            if (doubleJumpActive)
                readyToDoubleJump = true;


            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Doppio salto
        if (Input.GetKeyDown(jumpKey) && readyToDoubleJump && !grounded)
        {
            // Debug.Log("Doppio salto = " + readyToDoubleJump);

            Jump();

            readyToDoubleJump = false;
        }

        // Skill attiva
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (SkillManager.instance.skillReady)
            {
                //  Debug.Log("SKILL ATTIVATA");               

                SkillManager.instance.skill.Activate(SkillManager.instance.skillCooldown / 2);
                SkillManager.instance.skillReady = false;
                StartCoroutine(SkillManager.instance.Cooldown(SkillManager.instance.skillCooldown));
            }

            else Debug.Log("Non puoi attivare la skill. Aspetta fine cooldown.");
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

            if (!moveDirection.Equals(Vector3.zero))
            {
                isWalking = true;
                StepSound();
            }
            else
            {
                isWalking = false;
                audioSource.pitch = originalPitch;
            }

        }

        // in air
        else if (!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
    }

    private void StepSound()
    {
        if (footSteps.Length == 0) return;

        int index = Random.Range(0, footSteps.Length - 1);

        float stepRate = 10 - moveSpeed;

        audioSource.pitch = stepRate;


        if (isWalking)
        {
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(footSteps[index]);
            else return;

            Invoke(nameof(StepSound), footSteps[index].length / stepRate);
        }
        else
            return;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // play jump sound
        // AudioSource.PlayClipAtPoint(jumpSound, gameObject.transform.position);
        audioSource.PlayOneShot(jumpSound);

        //  Debug.Log("SALTATO");
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}