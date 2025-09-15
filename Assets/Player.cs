using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    private Animator animator;

    // Saut
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float ascendMultiplier = 2f;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float raycastDistance = 1.2f;

    // Combat
    public float attackRange = 1.5f;
    private CharacterStats stats;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                | RigidbodyConstraints.FreezeRotationZ
                | RigidbodyConstraints.FreezeRotationY;


        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }

        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {
        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Vérifier si un monstre est proche (avec le tag "Monster")
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Monster"))
            {
                CharacterStats enemyStats = col.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    Debug.Log("Le joueur attaque le monstre !");
                    stats.Attack(enemyStats);
                }
            }
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        bool isWalking = flatVelocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);

        bool isJumping = !isGrounded && rb.linearVelocity.y > 0.1f;
        animator.SetBool("IsJumping", isJumping);

        bool isFalling = !isGrounded && rb.linearVelocity.y < -0.1f;
        animator.SetBool("IsFalling", isFalling);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
