using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 6;
    public bool canMove = true;
    private Rigidbody2D rb;
    SpriteRenderer spriteRend;
    public Sprite[] sprites;
    bool moveUp;
    bool moveLeft;
    bool moveDown;
    bool moveRight;

    void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        
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
            spriteRend.sprite = sprites[0];
        }
        else if (!moveDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (moveLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            spriteRend.sprite = sprites[1];
        }
        else if (!moveRight)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (moveDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, -moveSpeed);
            spriteRend.sprite = sprites[2];
        }
        else if (!moveUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (moveRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            spriteRend.sprite = sprites[3];
        }
        else if (!moveLeft)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
