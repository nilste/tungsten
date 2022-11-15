using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage the player
/// </summary>
public class PlayerScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpForce = 850;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] int maxHealth = 3;

    [Header("References")]
    [SerializeField] AudioSource attackSFX = null;
    [SerializeField] AudioSource deathSFX = null;
    [SerializeField] AudioSource hurtSFX = null;
    [SerializeField] GameObject attackHitBox = null;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] ParticleSystem dustVFX = null;
    private Animator myAnimator;
    private BoxCollider2D myFeetCollider;
    private CapsuleCollider2D myBodyCollider;
    private Rigidbody2D myRB2D;

    // Variables
    private bool isAlive = true;
    private bool isAttacking = false;
    private bool isAttackedPressed = false;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isJumpPressed = false;
    private bool isParalyzed = false;
    private string currentAnimaton;
    private float xAxis = .0f;
    private int currentHealth;

    //Animation States
    const string PLAYER_IDLE = "PlayerIdle";
    const string PLAYER_RUN = "PlayerRun";
    const string PLAYER_JUMP = "PlayerJump";
    const string PLAYER_ATTACK = "PlayerAttack";
    const string PLAYER_AIR_ATTACK = "PlayerAirAttack";
    const string PLAYER_DEAD = "PlayerDead";

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myRB2D = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        attackHitBox.SetActive(false);
    }

    void Update()
    {
        if (!isAlive) { return; }

        xAxis = Input.GetAxis("Horizontal"); // value -1 to +1


        // isJumpPressed = Input.GetButtonDown("Jump");
        // Check grounded to avoid new jump pre landing
        if (Input.GetButtonDown("Jump"))
        {
            isJumpPressed = true;
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttackedPressed = true;
            StartCoroutine(PerformAttack());
        }

        ManageDamage();
    }

    private void FixedUpdate()
    {
        if (!isAlive || isParalyzed) { return; }

        // Move player based on X input
        myRB2D.velocity = new Vector2(xAxis * runSpeed, myRB2D.velocity.y);

        if (xAxis != 0)
        {
            // Orient the player horizontally, Sign() returns a value of +1 or -1
            // Only when xAxis is not 0, to avoid automatically turning right
            transform.localScale = new Vector3(Mathf.Sign(xAxis), 1f, 1f);
        }
        
        isGrounded = IsGroundedCheck();

        if (isGrounded && !isJumping)
        {
            // Manage regular running/idle
            if (!isAttacking)
            {
                if (xAxis != 0)
                {
                    ChangeAnimationState(PLAYER_RUN);
                    dustVFX.Play();
                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE);
                }
            }

            // Manage jumping
            if (isJumpPressed)
            {
                isJumping = true;
                isJumpPressed = false;
                myRB2D.AddForce(new Vector2(0, jumpForce));
                ChangeAnimationState(PLAYER_JUMP);
                dustVFX.Play();

                // Allow time to get up in air before next ground check
                Invoke("EnableJumping", .1f);
            }
        }

        if (isAttackedPressed)
        {
            isAttackedPressed = false;
            
            if (!isAttacking)
            {
                isAttacking = true;

                if (isGrounded)
                {
                    ChangeAnimationState(PLAYER_ATTACK);
                }
                else
                {
                    ChangeAnimationState(PLAYER_AIR_ATTACK);
                }

                Invoke("AttackCompleted", attackCooldown);
            }
        }
    }

    IEnumerator PerformAttack()
    {
        attackSFX.Play();
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(.4f);
        attackHitBox.SetActive(false);
    }

    void AttackCompleted()
    {
        isAttacking = false;
    }

    void KnockBackCompleted()
    {
        isParalyzed = false;
    }

    void EnableJumping()
    {
        isJumping = false;
    }

    private void ManageDamage()
    {
        if (isAlive)
        {
            if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
            {
                currentHealth = 0;
            }

            if (currentHealth < 1)
            {
                StartCoroutine(ProcessDeath());
            }
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        myAnimator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    IEnumerator PlayDeathVFX()
    {
        // Avoid spawning VFX mid-air
        yield return new WaitForSeconds(1f);
        Instantiate(deathVFX, transform.position + new Vector3(0f,2f,0f), transform.rotation);
    }

    IEnumerator ProcessDeath()
    {
        isAlive = false;
        deathSFX.Play();

        // Stop the player from sliding after death
        myRB2D.velocity = new Vector2(0, -3f);
        myRB2D.drag = 1f;

        ChangeAnimationState(PLAYER_DEAD);

        StartCoroutine(PlayDeathVFX());

        // Give time to play death animation before going forward
        yield return new WaitForSeconds(4f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    public void TakeDamage(int amountOfDamage)
    {
        if (isAlive)
        {
            currentHealth -= amountOfDamage;
            hurtSFX.Play();

            // Knockback
            isParalyzed = true;
            myRB2D.AddForce(-myRB2D.velocity * 100);
            Invoke("KnockBackCompleted", .5f);
        }
    }

    private bool IsGroundedCheck() {
        float extraHeightTest = .4f;

        // Throw a BoxCast to see if it hits ground layer
        // Size is slightly smaller to avoid walljumping
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            myBodyCollider.bounds.center,
            myBodyCollider.bounds.size - new Vector3(0.1f, 0f, 0f),
            0f,
            Vector2.down,
            extraHeightTest,
            LayerMask.GetMask("Ground")
        );

        return raycastHit.collider != null;
    }

    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public void AddToHealth(int value) { currentHealth += value; }
    public bool IsAlive() { return isAlive; }
}
