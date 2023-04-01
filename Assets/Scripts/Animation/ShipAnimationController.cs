using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationController : MonoBehaviour
{
    private Animator animator;
    private int tasksComplete = 0;
    private static ShipAnimationController instance;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ERROR: more than one ShipAnimationController in scene!");
        }
        instance = this;
    }

    public void SetMiniGameComplete()
    {
        tasksComplete = tasksComplete + 1;
        animator.SetInteger("tasksComplete", tasksComplete);
    }

    public static ShipAnimationController GetInstance()
    {
        return instance;
    }
}
