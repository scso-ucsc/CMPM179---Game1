using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianManager : MonoBehaviour
{
    //Code made with help of https://www.youtube.com/watch?v=sIf_SQzj054 - Creating an Enemy Wander AI (Unity Tutorial | 2D Top Down Shooter)

    [SerializeField] private float movementSpeed, rotationSpeed, movementBounds;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float directionChangeTime;
    private Quaternion targetRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.up;
        targetRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        updateTargetDirection();
        rotateBody();
        setVelocity();
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
            directionChangeTime = Random.Range(1f, 5f);
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
        if (position.x < -movementBounds || position.x > movementBounds || position.y < -movementBounds || position.y > movementBounds)
        {
            //Make Object Return To Bounds
        }
    }
}
