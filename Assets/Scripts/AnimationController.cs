using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Walking

        animator.SetFloat("walkSpeed",Mathf.Abs(rb.velocity.x));

        // Flip sprite according to movement direction

       if(Mathf.Abs(rb.velocity.x) > 0){
        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

       }
       
       // Jumping
       // The threshold is set to 1 to avoid the issue caused by the constant tiny y-velocity value even when the player is standing still.
        
        if (rb.velocity.y > 1)
       {
        animator.SetBool("isJumping", true);
       }
       else
       {
        animator.SetBool("isJumping", false);
       }
    }

}
