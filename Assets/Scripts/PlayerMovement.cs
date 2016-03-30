﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 6;
    public bool canMove = true;
    private Rigidbody2D rb;

    bool moveUp;
    bool moveLeft;
    bool moveDown;
    bool moveRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	void Update ()
    {
        if (Input.GetKey(KeyCode.W) && canMove)
        {
            moveUp = true;
        }
        else
        {
            moveUp = false;
        }
        if (Input.GetKey(KeyCode.A) && canMove)
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }
        if (Input.GetKey(KeyCode.S) && canMove)
        {
            moveDown = true;
        }
        else
        {
            moveDown = false;
        }
        if (Input.GetKey(KeyCode.D) && canMove)
        {
            moveRight = true;
        }
        else
        {
            moveRight = false;
        }
	}
    void FixedUpdate()
    {
        if (moveUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
        }
        else if (!moveDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (moveLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (!moveRight)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (moveDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
        }
        else if (!moveUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (moveRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if (!moveLeft)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
