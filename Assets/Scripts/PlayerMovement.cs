﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 6;
    public bool canMove = true;
    private Rigidbody2D rb;
    Animator anim;
    public Sprite[] sprites;
    bool moveUp;
    bool moveLeft;
    bool moveDown;
    bool moveRight;

    public KeyCode upKey;
    public KeyCode leftKey;
    public KeyCode downKey;
    public KeyCode rightKey;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

	void Update ()
    {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movementVector != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("inputX", movementVector.x);
            anim.SetFloat("inputY", movementVector.y);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        //Move up
        if (Input.GetKey(upKey) && canMove)
        {
            moveUp = true;          
        }
        else
        {
            moveUp = false;
        }

        //Move down
        if (Input.GetKey(downKey) && canMove)
        {
            moveDown = true;
        }
        else
        {
            moveDown = false;
        }

        //Move left
        if (Input.GetKey(leftKey) && canMove)
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }

        //Move right
        if (Input.GetKey(rightKey) && canMove)
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
