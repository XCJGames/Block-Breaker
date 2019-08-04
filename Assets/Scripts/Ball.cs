using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // congif params
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush= 15f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // state
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
        else
        {
            if(myRigidBody2D.velocity.x == 0 || myRigidBody2D.velocity.y == 0)
            {
                unstuckBall();
            }
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(1.04f, 0.694f);
        hasStarted = false;
    }

    private void unstuckBall()
    {
        Vector2 newVelocity = new Vector2
            (UnityEngine.Random.Range(-10f,-12f),
            UnityEngine.Random.Range(-10f, -12f));
        myRigidBody2D.velocity = newVelocity;
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
            hasStarted = true;
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (UnityEngine.Random.Range(0f, randomFactor),
            UnityEngine.Random.Range(0f, randomFactor));
        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;
            checkVelocity();
        }
    }

    private void checkVelocity()
    {
        if((Mathf.Abs(myRigidBody2D.velocity.x) + Mathf.Abs(myRigidBody2D.velocity.y)) < 12f)
        {
            float xVel;
            float yVel;
            if(myRigidBody2D.velocity.x >= 0)
            {
                xVel = UnityEngine.Random.Range(4f, 8f);
            }
            else
            {
                xVel = UnityEngine.Random.Range(-4f, -8f);
            }
            if (myRigidBody2D.velocity.y >= 0)
            {
                yVel = UnityEngine.Random.Range(4f, 8f);
            }
            else
            {
                yVel = UnityEngine.Random.Range(-4f, -8f);
            }
            Vector2 newVelocity = new Vector2
            (xVel,
            yVel);
            myRigidBody2D.velocity = newVelocity;
        }
        else if(myRigidBody2D.velocity.y == 0)
        {
            Vector2 newVelocity = new Vector2
            (0,
            UnityEngine.Random.Range(-2f, -10f));
            myRigidBody2D.velocity += newVelocity;
        }
    }
}
