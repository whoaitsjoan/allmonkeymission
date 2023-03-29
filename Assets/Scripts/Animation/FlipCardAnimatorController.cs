using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipCardAnimatorController : MonoBehaviour
{
    Animator anim;
    private Button button;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetFlippingTrue() 
    {
        anim.SetBool("flipping", true);
        Debug.Log("Flipping set true!");
    }

    public void SetFlippingFalse()
    {
        anim.SetBool("flipping", false);
    }
}
