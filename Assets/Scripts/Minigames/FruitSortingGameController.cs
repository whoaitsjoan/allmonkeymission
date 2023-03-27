using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSortingGameController : MonoBehaviour
{
    private int livesLeft = 5; //start with 5 lives at the beginning


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //Just for debugging purposes right now
    public void OnCardClick() 
    {
        Debug.Log("Card clicked!");
    }

    public bool CheckMatch()
    {
        // if fruit types match, deactivate buttons
        // else, decrease lives and flip cards back over

        return true; // temporary return statement so compiler doesn't yell at me
    }


    // If there isn't a match, decrease the number of lives
    public void DecreaseLives()
    {
        if (livesLeft > 0)
        {
            livesLeft = livesLeft - 1;
            
            // change monkey tint!
            // might need to get a list of the monkey lives (last in, first out)
        }
        else
        {
            Debug.Log("ERROR: No lives left!");
        }
    }
}
