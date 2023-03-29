using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FruitSortingGameController : MonoBehaviour
{
    [SerializeField] GameObject gameObject;
    private bool gameOver = false;

    private int livesLeft = 5; //start with 5 lives at the beginning
    [SerializeField] private GameObject livesObject;
    private List<Image> livesList = new List<Image>();

    private List<string> fruitNameList = new List<string>() {"apple","apple","banana","banana","grapes","grapes","strawberry","strawberry","watermelon","watermelon","orange","orange","pineapple","pineapple","peach","peach"};
    private List<FruitCard> cardList = new List<FruitCard>();
    private List<FruitCard> flippedCardsList = new List<FruitCard>();
    private List<Button> pressedButtonsList = new List<Button>();
    [SerializeField] Sprite[] fruitSprites; // array of fruits

    [SerializeField] FlipCardAnimatorController flipCardAnimatorController;

    private void Awake()
    {
        // Get list of lives
        foreach (Image item in livesObject.GetComponentsInChildren<Image>()) 
        {
            livesList.Add(item);
        }

        // Randomize fruit cards
        RandomizeCards();
    }

    #region CardSetupMethods
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

    private void SetFruitSprite(FruitCard fc, Image img)
    {
        switch (fc.fruitType)
        {
            case "apple":
                img.sprite = fruitSprites[0];
                break;
            case "banana":
                img.sprite = fruitSprites[1];
                break;
            case "grapes":
                img.sprite = fruitSprites[2];
                break;
            case "strawberry":
                img.sprite = fruitSprites[3];
                break;
            case "watermelon":
                img.sprite = fruitSprites[4];
                break;
            case "orange":
                img.sprite = fruitSprites[5];
                break;
            case "pineapple":
                img.sprite = fruitSprites[6];
                break;
            case "peach":
                img.sprite = fruitSprites[7];
                break;
        };
    }

    private IEnumerator ResetCardOnMismatch(Button button)
    {
        Image buttonImg = button.GetComponent<Image>();
        // TO DO: other cards shouldn't be interactable in this time
        yield return new WaitForSeconds(1f); // pause so players can see cards
        buttonImg.sprite = fruitSprites[8];
        button.interactable = true; // reactivates buttons if not a match
    }
    #endregion

    #region GameplayMethods
    public void OnCardClick(Button button) 
    {
        if (gameOver)
        {
            return;
        }
        
        string buttonString = button.name;
        int buttonInt = (int) Int64.Parse(buttonString); // converts button name string to int
        Debug.Log(buttonInt + " was clicked!");

        FruitCard flippedCard = ScriptableObject.CreateInstance<FruitCard>();
        flippedCard = cardList[buttonInt];
        flippedCard.isFlipped = true;
        flippedCardsList.Add(flippedCard);
        pressedButtonsList.Add(button);

        Image buttonImg = button.GetComponent<Image>();
        SetFruitSprite(flippedCard, buttonImg);
        flipCardAnimatorController = button.GetComponent<FlipCardAnimatorController>();
        flipCardAnimatorController.SetFlippingTrue();
        button.interactable = false;
        
        // for debugging only
        foreach (FruitCard fc in flippedCardsList)
        {
            Debug.Log(fc.FruitCardToString());
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

    private void CheckMatch()
    {
        // if fruit types match, deactivate buttons
        FruitCard firstCard = flippedCardsList[0];
        FruitCard secondCard = flippedCardsList[1];

        if (firstCard.fruitType == secondCard.fruitType)
        {
            Debug.Log("Match!");
            // add animation & animation event to call this foreach in separate function
            foreach (Button correctMatch in pressedButtonsList)
            {
                StartCoroutine(DeactivateCardsOnMatch(correctMatch));
            }
        }
        else // else, decrease lives and flip cards back over
        {
            Debug.Log("Not a match!");
            DecreaseLives();
            foreach (Button falseMatch in pressedButtonsList)
            {
                StartCoroutine(ResetCardOnMismatch(falseMatch));     
            }
        }

        flippedCardsList.Clear();
        pressedButtonsList.Clear();
    }

    private IEnumerator DeactivateCardsOnMatch(Button button)
    {
        // TO DO: other cards shouldn't be interactable in this time
        yield return new WaitForSeconds(1f);
        button.gameObject.SetActive(false);
    }

    // If there isn't a match, decrease the number of lives
    private void DecreaseLives()
    {
        if (livesLeft > 0 && livesList.Count > 0)
        {
            livesLeft = livesLeft - 1;
            livesList.Last().color = Color.black;
            livesList.Remove(livesList.Last());
            if (livesLeft == 0)
            {
                gameOver = true;
                GameOver();
            }
        }
        else
        {
            Debug.Log("ERROR: No lives left!");
        }
    }

    private void SetAllOtherButtonsInactive()
    {
        // make all other buttons non-interactable when checking if match
        // if match, do nothing (buttons will be deactivated)
        // else, make sure clicked buttons are reset to interactable if not matching
    }

    private void GameOver()
    {
        // add game over panel
        gameObject.SetActive(false);
    }
    #endregion
}