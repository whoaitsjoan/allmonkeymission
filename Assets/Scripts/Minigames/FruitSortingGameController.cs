using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FruitSortingGameController : MonoBehaviour
{
    private int livesLeft = 5; //start with 5 lives at the beginning
    public GameObject livesObject;
    private List<Image> livesList = new List<Image>();

    private List<string> fruitNameList = new List<string>() {"apple","apple","banana","banana","grapes","grapes","strawberry","strawberry","watermelon","watermelon","orange","orange","pineapple","pineapple","peach","peach"};
    private List<FruitCard> cardList = new List<FruitCard>();
    private List<FruitCard> flippedCardsList = new List<FruitCard>();


    // Start is called before the first frame update
    private void Start()
    {
        // Get list of lives
        foreach (Image item in livesObject.GetComponentsInChildren<Image>() ) {
            livesList.Add(item);
        }

        // Randomize fruit cards
        RandomizeCards();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void RandomizeCards() 
    {
        for (int i = 0; i < 16; i++) // 16 cards
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

    private string GenerateFruitType()
    {
        int index = UnityEngine.Random.Range(0, fruitNameList.Count);
        string fruitName = fruitNameList.ElementAt(index);
        fruitNameList.RemoveAt(index); // remove this fruit from the list
        return fruitName;
    }

    public void OnCardClick(Button button) 
    {
        string buttonString = button.name;
        int buttonInt = (int) Int64.Parse(buttonString); // converts button name string to int
        Debug.Log(buttonInt + " was clicked!");

        FruitCard flippedCard = ScriptableObject.CreateInstance<FruitCard>();
        flippedCard = cardList[buttonInt];
        flippedCardsList.Add(flippedCard);
        foreach (FruitCard item in flippedCardsList)
        {
            Debug.Log(item.FruitCardToString());
        }

        // Flip animation
        if (flippedCardsList.Count == 2) // if it is the second card
        {
            // don't allow clicking other buttons (low priority)
            CheckMatch();
        }
        else if (flippedCardsList.Count > 2)
        {
            Debug.Log("ERROR: max two cards can be flipped at a time");
        }
        
    }

    private bool CheckMatch()
    {
        // if fruit types match, deactivate buttons
        FruitCard firstCard = flippedCardsList[0];
        FruitCard secondCard = flippedCardsList[1];

        if (firstCard.fruitType == secondCard.fruitType)
        {
            Debug.Log("Match!");
            flippedCardsList.Clear();
            return true;
        }
        else // else, decrease lives and flip cards back over
        {
            Debug.Log("Not a match!");
            flippedCardsList.Clear();
            DecreaseLives();
            return false;
        }
    }


    // If there isn't a match, decrease the number of lives
    private void DecreaseLives()
    {
        Color lostLife = new Color(132,132,132);
        if (livesLeft > 0 && livesList.Count > 0)
        {
            livesLeft = livesLeft - 1;
            livesList.Last().color = lostLife;
            livesList.Remove(livesList.Last());
            // might need to get a list of the monkey lives (last in, first out)
        }
        else
        {
            Debug.Log("ERROR: No lives left!");
        }
    }
}
