using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move hazard back and forward on X and Y axis
/// </summary>
public class MovingHazard : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveDistance = 0f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool shouldMoveX = false;
    [SerializeField] bool shouldMoveY = false;
    [SerializeField] bool isMovingUp = false;
    [SerializeField] bool isMovingRight = false;
    // [SerializeField] bool isActive = false;
    
    private Rigidbody2D myRB2D;
    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
        myRB2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Get current distance from starting position
        float distance = Vector3.Distance(startingPosition, transform.position);

        // Reached end position, turn around
        if (moveDistance < distance)
        {
            isMovingUp = !isMovingUp;
            isMovingRight = !isMovingRight;
        }

        if (shouldMoveX) {
            if (isMovingRight)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }

        if (shouldMoveY) {
            if (isMovingUp)
            {
                MoveUp();
            }
            else
            {
                MoveDown();
            }
        }
    }

    private void MoveRight()
    {
        myRB2D.velocity = new Vector2(moveSpeed, myRB2D.velocity.y);
    }

    private void MoveLeft()
    {
        myRB2D.velocity = new Vector2(-moveSpeed, myRB2D.velocity.y);
    }

    private void MoveUp()
    {
        myRB2D.velocity = new Vector2(myRB2D.velocity.x, moveSpeed);
    }

    private void MoveDown()
    {
        myRB2D.velocity = new Vector2(-myRB2D.velocity.x, moveSpeed);
    }
}
