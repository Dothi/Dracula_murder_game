using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float moveSpeed = 6;
    public bool canMove = true;
    private Rigidbody2D rb;
    SpriteRenderer spriteRend;
    Animator anim;
    public Sprite[] sprites;
    bool moveUp;
    bool moveLeft;
    bool moveDown;
    bool moveRight;

    void Start()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        
        rb = GetComponent<Rigidbody2D>();
    }

	void Update ()
    {
        if (rb.velocity != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }


        if (Input.GetKey(KeyCode.W) && canMove)
        {
            moveUp = true;
            anim.SetFloat("inputY", 1f);
        }
        else
        {
            moveUp = false;
        }
        if (Input.GetKey(KeyCode.A) && canMove)
        {
            moveLeft = true;
            anim.SetFloat("inputX", -1f);
        }
        else
        {
            moveLeft = false;
        }
        if (Input.GetKey(KeyCode.S) && canMove)
        {
            moveDown = true;
            anim.SetFloat("inputY", -1f);
        }
        else
        {
            moveDown = false;
        }
        if (Input.GetKey(KeyCode.D) && canMove)
        {
            moveRight = true;
            anim.SetFloat("inputX", 1f);
        }
        else
        {
            moveRight = false;
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            anim.SetFloat("inputY", 0f);
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("inputX", 0f);
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
