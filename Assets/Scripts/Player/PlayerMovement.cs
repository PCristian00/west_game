using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    bool readyToJump;
    // public int jumpLimit = 1;
    private bool readyToDoubleJump = false;
    public bool doubleJumpActive = false;

    // FORSE NON IMPLEMENTATE
    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Sound")]
    public AudioClip jumpSound;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    // Migliorare nomenclatura / struttura
    // mettere boolean che non fa attivare se ancora attiva
    // CREARE SKILL MANAGER? (static instance???)

    [Header("Skills")]
    public GameObject activeSkill;
    private IPowerup skill;
    public float skillCooldown;
    private bool skillReady = true;
    public Slider skillBar;




    private void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        skill = activeSkill.GetComponent<IPowerup>();
        Debug.Log(skill);


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
            if (skillReady)
            {
                Debug.Log("SKILL ATTIVATA");
                // Qui va passato quanto tempo rimane attivata la skill.
                // DOVREBBE ESSERE MINORE DI skillCooldown
                skill.Activate(skillCooldown / 2);
                skillReady = false;

                StartCoroutine(Cooldown(skillCooldown));
            }
            else Debug.Log("Non puoi attivare la skill. Aspetta fine cooldown.");
        }
    }

    private IEnumerator Cooldown(float time)
    {
        skillBar.gameObject.SetActive(true);

        float t = 0f;


        //yield return new WaitForSeconds(time);       


        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime / time;

            // float tvalue = Mathf.Clamp01(t / .9f);

            // TEST: Se il power up è finito (cooldown diviso 2 attualmente) la barra cambia colore fino a fine
            //if (t <= 0.5f)
            //{
                
                
            //}

           // skillBar.value = tvalue;
            skillBar.value = t;
            // Inserire qui ripristino icona, messaggio etc...
            // Provare barra di caricamento 

        }

        skillBar.gameObject.SetActive(false);

        Debug.Log("Skill PRONTA");
        skillReady = true;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
            // IMPLEMENTARE SUONO PASSI??
        }


        // in air (FORSE RIMUOVERE - FA FARE SCHIVATE IN ARIA)
        else if (!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
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
        AudioSource.PlayClipAtPoint(jumpSound, gameObject.transform.position);

        //  Debug.Log("SALTATO");
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}