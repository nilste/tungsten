using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic enemy functionality
/// </summary>
public class EnemyBase : MonoBehaviour
{
    [Header("Settings")]
    // [SerializeField] bool shouldMoveY = false;
    // [SerializeField] bool isMovingUp = false;
    [SerializeField] int amountDamage = 1;
    [SerializeField] int maxHealth = 20;
    [SerializeField] bool followPlayer = false;
    [SerializeField] bool isAwake = false;
    [SerializeField] bool isMovingRight = false;
    [SerializeField] bool shouldMoveX = false;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float attackDistance = 2f;
    [SerializeField] float discoverDistance = 10f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveDistance = 5f;
    private bool isAlive = true;
    private bool isAttacking = false;
    private bool isHalted = false;
    private int currentHealth;

    [Header("References")]
    [SerializeField] AudioSource attackSFX = null;
    [SerializeField] AudioSource awakeSFX = null;
    [SerializeField] AudioSource deathSFX = null;
    [SerializeField] AudioSource hurtSFX = null;
    [SerializeField] GameObject attackObject = null;
    [SerializeField] GameObject deathVFX = null;
    private GameObject playerTarget;
    private GameObject playerObject;
    private Animator myAnimator;
    private Rigidbody2D myRB2D;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
        currentHealth = maxHealth;
        playerObject = GameObject.FindWithTag("Player");

        myRB2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isAlive) { return; }

        // Use starting position as reference point
        float playerDistance = Vector3.Distance(startingPosition, playerObject.transform.position);

        // Wake up if object is sleeping and player is within distance
        if (!isAwake)
        {
            if (playerDistance < discoverDistance)
            {
                isAwake = true;
                if (awakeSFX != null) { awakeSFX.Play(); }
            }
            else
            {
                return;
            }
        }

        // Reset the enemy before making moving decisions
        myRB2D.velocity = Vector3.zero;

        // Manage walking
        if (!isHalted)
        {
            if (followPlayer && playerDistance < moveDistance)
            {
                WalkTowardsPlayer();
            }
            else
            {
                WalkTowardsWaypoint();
            }
        }

        // Manage attack
        float localPlayerDistance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (!isAttacking && localPlayerDistance < attackDistance &&
            playerObject.GetComponent<PlayerScript>().IsAlive())
        {
            isHalted = true;
            isAttacking = true;

            myAnimator.SetTrigger("isAttacking");
            if (attackSFX != null) { attackSFX.Play(); }

            if (attackObject != null)
            {
                attackObject.SendMessage("PerformAttack", attackCooldown);
            }

            StartCoroutine(EnableAttacking());
            StartCoroutine(EnableWalk());
        }

        // Set bool for move animation
        bool isMoving = myRB2D.IsAwake();
        myAnimator.SetBool("isMoving", isMoving);
    }

    private void WalkTowardsPlayer()
    {
        // Only move closer up to a certain point
        if (Vector3.Distance(transform.position, playerObject.transform.position) > attackDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector2(playerObject.transform.position.x, transform.position.y),
                moveSpeed * Time.deltaTime
            );
        }

        // Orient enemy towards player
        float distanceDifference = transform.position.x - playerObject.transform.position.x;
        transform.localScale = new Vector3(-(Mathf.Sign(distanceDifference)), 1f, 1f);
    }

    private void WalkTowardsWaypoint()
    {
        float positionDistance = Vector3.Distance(startingPosition, transform.position);

        // Reached end position, turn around
        if (moveDistance < positionDistance)
        {
            // isMovingUp = !isMovingUp;
            isMovingRight = !isMovingRight;
        }

        if (shouldMoveX) {
            if (isMovingRight)
            {
                MoveRight();
                transform.localScale = new Vector3(1f, 1f, 1f);

            }
            else
            {
                MoveLeft();
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        // if (shouldMoveY) {
        //     if (isMovingUp)
        //     {
        //         MoveUp();
        //     }
        //     else
        //     {
        //         MoveDown();
        //     }
        // }
    }

    private void MoveRight()
    {
        myRB2D.velocity = new Vector2(moveSpeed, myRB2D.velocity.y);
    }

    private void MoveLeft()
    {
        myRB2D.velocity = new Vector2(-moveSpeed, myRB2D.velocity.y);
    }

    // private void MoveUp()
    // {
    //     myRB2D.velocity = new Vector2(myRB2D.velocity.x, moveSpeed);
    // }

    // private void MoveDown()
    // {
    //     myRB2D.velocity = new Vector2(-myRB2D.velocity.x, moveSpeed);
    // }

    public int GetAmountDamage()
    {
        return amountDamage;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (isAlive)
        {
            if (currentHealth > 0)
            {
                if (hurtSFX != null) { hurtSFX.Play(); }
                myAnimator.SetTrigger("isHurting");
            }
            else
            {
                ProcessDeath();            
            }
        }
    }

    private void ProcessDeath()
    {
        isAlive = false;

        GetComponent<Collider2D>().enabled = false;
        
        myRB2D.velocity = Vector3.zero;
        myRB2D.angularVelocity = 0;
        
        myAnimator.SetTrigger("isDead");

        if (attackSFX != null) { attackSFX.Stop(); }
        if (deathSFX != null) { deathSFX.Play(); }

        StartCoroutine(PlayDeathVFX());
    }

    IEnumerator PlayDeathVFX()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator EnableAttacking()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    IEnumerator EnableWalk()
    {
        yield return new WaitForSeconds(.5f);
        isHalted = false;
    }
}
