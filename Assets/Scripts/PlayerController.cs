using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    private Rigidbody2D rb;

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
        Vector2 newVelocity = rb.velocity; 

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newVelocity.x = -walkSpeed;
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            newVelocity.x = walkSpeed;
        }
        
        else
        {
            newVelocity.x = 0;
        }

        rb.velocity = newVelocity;
    }
}