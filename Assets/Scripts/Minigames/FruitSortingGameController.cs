using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FruitSortingGameController : MonoBehaviour
{
    private int livesLeft = 5; //start with 5 lives at the beginning
    public GameObject livesObject;
    List<Image> livesList = new List<Image>();
    List<string> fruitNameList = new List<string>() {"apple","apple","banana","banana","grapes","grapes","strawberry","strawberry","watermelon","watermelon","orange","orange","pineapple","pineapple","peach","peach"};
    List<FruitCard> cardList = new List<FruitCard>();
    List<FruitCard> flippedCards = new List<FruitCard>();


    // Start is called before the first frame update
    void Start()
    {
        // Get list of lives
        foreach (Image item in livesObject.GetComponentsInChildren<Image>() ) {
            livesList.Add(item);
        }

        // Randomize fruit cards
        RandomizeCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeCards() 
    {
        for (int i = 0; i < 16; i++)
        {
            FruitCard fc = ScriptableObject.CreateInstance<FruitCard>();
            string fruitType = GenerateFruitType();
            fc.SetFruitType(fruitType);
            cardList.Add(fc);
        }
        foreach (FruitCard fc in cardList)
        {
            Debug.Log(fc.FruitCardToString());
        }
    }

    public string GenerateFruitType()
    {
        int index = Random.Range(0, fruitNameList.Count);
        string fruitName = fruitNameList.ElementAt(index);
        fruitNameList.RemoveAt(index); // remove this fruit from the list
        return fruitName;
    }

    
    //Just for debugging purposes right now
    public void OnCardClick() 
    {
        Debug.Log("Card clicked!");
        // Flip animation
        // if is first card, wait for second card to be flipped
        // if is second card, don't allow more clicking & check if match
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
        if (livesLeft > 0 && livesList.Count > 0)
        {
            livesLeft = livesLeft - 1;
            livesList.Remove(livesList.Last());
            // change monkey tint!
            // might need to get a list of the monkey lives (last in, first out)
        }
        else
        {
            Debug.Log("ERROR: No lives left!");
        }
    }
}
