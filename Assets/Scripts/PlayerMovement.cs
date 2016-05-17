using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    GameController gc;
    public float moveSpeed = 6;
    public bool canMove = true;
    private Rigidbody2D rb;
    Animator anim;
    public Sprite[] sprites;
    bool moveUp;
    bool moveLeft;
    bool moveDown;
    bool moveRight;

    GameObject pauseMenu;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

	void Update ()
    {
        Debug.Log(Time.timeScale);
        if (!gc.gameOver || pauseMenu.activeInHierarchy)
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
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GetComponent<KillScript>().isSuckingBlood && !gc.gameOver)
            {
                Time.timeScale = 0f;
                gc.GetComponent<BloodBar>().enabled = false;
                pauseMenu.SetActive(true);
            }
        }  

        //Move up
        if (Input.GetAxis("Vertical") > 0 && canMove)
        {
            moveUp = true;          
        }
        else
        {
            moveUp = false;
        }

        //Move down
        if (Input.GetAxis("Vertical") < 0 && canMove)
        {
            moveDown = true;
        }
        else
        {
            moveDown = false;
        }

        //Move left
        if (Input.GetAxis("Horizontal") < 0 && canMove)
        {
            moveLeft = true;
        }
        else
        {
            moveLeft = false;
        }

        //Move right
        if (Input.GetAxis("Horizontal") > 0 && canMove)
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
