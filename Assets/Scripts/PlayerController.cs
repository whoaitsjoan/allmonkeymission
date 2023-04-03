using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float jumpHeight;
    private Rigidbody2D rb;
    private bool canJump;
    private static PlayerController instance;
    private bool inMinigame = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ERROR: more than one PlayerController in scene!");
        }
        instance = this;
    }

    public static PlayerController GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump == true)
        {
            rb.velocity = Vector2.up * jumpHeight;
        }

        UpdateVelocity();
    }

    private void UpdateVelocity()
    {
        Vector2 newVelocity = rb.velocity; 

        if (Input.GetKey(KeyCode.LeftArrow) && !inMinigame)
        {
            newVelocity.x = -walkSpeed;
        }

        else if (Input.GetKey(KeyCode.RightArrow) && !inMinigame)
        {
            newVelocity.x = walkSpeed;
        }
        
        else
        {
            newVelocity.x = 0;
        }

        rb.velocity = newVelocity;
    }

    public void PlayingMinigame()
    {
        inMinigame = true;
    }

    public void EndMinigame()
    {
        inMinigame = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If we collide with an object tagged "ground" then our jump resets and we can now jump.
        if (collision.gameObject.tag == "ground")
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //If we exit our collision with the "ground" object, then we are unable to jump.
        if (collision.gameObject.tag == "ground")
        {
            canJump = false;
        }
    }
   
}