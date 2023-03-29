using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipCardAnimatorController : MonoBehaviour
{
    private Animator anim;
    public FruitSortingGameController fruitSortingGameController;
    
    /* 
    *  keep references to fruitCard flipped & buttonImage for CallSetFruitSprite
    *  because AnimationEvents don't allow functions with params other than int, float, string
    */
    private FruitCard fc;
    private Image buttonImg;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetFlippingTrue(FruitCard fruitCard, Image buttonImage) 
    {
        anim.SetBool("flipping", true);
        Debug.Log("Flipping set true!");
        fc = fruitCard;
        buttonImg = buttonImage;
    }

    public void SetFlippingFalse()
    {
        anim.SetBool("flipping", false);
    }

    public void CallSetFruitSprite() // created to call in AnimationEvent
    {
        fruitSortingGameController.SetFruitSprite(fc, buttonImg);
    }
}
