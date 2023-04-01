using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    private Rigidbody2D rb;
    private static PlayerController instance;
    private bool canWalk = true;

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
        UpdateVelocity();
        
    }

    private void UpdateVelocity()
    {
        if (canWalk){
            Vector2 newVelocity = new Vector2(rb.velocity.x, 0);

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newVelocity = -walkSpeed;
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                newVelocity = walkSpeed;
            }
            
            else
            {
                newVelocity = 0;
            }

            //rb.velocity.x = newVelocityX;
        }
    }

    public void PlayingMinigame()
    {
        canWalk = false;
    }

    public void EndMinigame()
    {
        canWalk = true;
    }

   
}