using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianManager : MonoBehaviour
{
    //Code made with help of https://www.youtube.com/watch?v=sIf_SQzj054 - Creating an Enemy Wander AI (Unity Tutorial | 2D Top Down Shooter)

    [SerializeField] private float movementSpeed, rotationSpeed, xMin, xMax, yMin, yMax;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float directionChangeTime;
    private bool returningToCentre;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.up;
        returningToCentre = false;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.getGameOverStatus() == false)
        {
            if (returningToCentre == true)
            {
                returnToCentre();
            }
            else
            {
                updateTargetDirection();
                rotateBody();
                setVelocity();
            }
        }
    }

    private void updateTargetDirection()
    {
        changeDirection();
        handleOutOfBounds();
    }

    private void changeDirection()
    {
        directionChangeTime -= Time.deltaTime;
        if (directionChangeTime <= 0)
        {
            float newAngle = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(newAngle, transform.forward);
            direction = rotation * direction;
            directionChangeTime = Random.Range(1f, 3f);
        }
    }

    private void rotateBody()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, direction);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.SetRotation(rotation);
    }

    private void setVelocity()
    {
        rb.velocity = transform.up * movementSpeed;
    }

    private void handleOutOfBounds()
    {
        Vector2 position = rb.position;
        if (position.x < xMin || position.x > xMax || position.y < yMin || position.y > yMax)
        {
            returningToCentre = true;
        }
    }

    private void returnToCentre()
    {
        Vector2 targetPosition = Vector2.zero;
        direction = (targetPosition - rb.position).normalized;

        setVelocity();
        rotateBody();

        if (Vector2.Distance(rb.position, targetPosition) < 0.5f)
        {
            returningToCentre = false;
            rb.velocity = Vector2.zero;
        }
    }
}
