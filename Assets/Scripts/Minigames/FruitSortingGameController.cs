using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitSortingGameController : MonoBehaviour
{    
    // Button variables
    [SerializeField] GameObject buttonsParentObject;
    int activeButtonsCount = 0; // gets populated in Awake()

    // Game State (Win or Lose) Variables
    [SerializeField] GameObject triesParentObject;
    [SerializeField] TMP_Text gameWonText;
    private bool outOfMatches = false;
    private bool outOfLives = false;
    [SerializeField] TMP_Text outOfLivesText;
    [SerializeField] Button restartButtonObject;
    [SerializeField] GameObject minigameObject;
    private int livesLeft = 0; // gets populated in Awake()
    [SerializeField] private GameObject livesObject;
    private List<Image> livesList = new List<Image>();
    public bool taskComplete = false;

    // FruitCard variables
    private List<string> fruitNameList = new List<string>() {"apple","apple","banana","banana","grapes","grapes","strawberry","strawberry","watermelon","watermelon","orange","orange","pineapple","pineapple","peach","peach"};
    private List<FruitCard> cardList = new List<FruitCard>();
    private List<FruitCard> flippedCardsList = new List<FruitCard>();
    private List<Button> pressedButtonsList = new List<Button>();
    [SerializeField] Sprite[] fruitSprites; // array of fruits
    private FlipCardAnimatorController flipCardAnimatorController;

    private void Awake()
    {        
        foreach (Image item in buttonsParentObject.GetComponentsInChildren<Image>()) 
        {
            activeButtonsCount = activeButtonsCount + 1;
        }
        
        // Get list of lives
        foreach (Image item in livesObject.GetComponentsInChildren<Image>()) 
        {
            livesList.Add(item);
            livesLeft = livesLeft + 1;
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

    public void SetFruitSprite(FruitCard fc, Image img)
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
        yield return new WaitForSeconds(2f); // pause so players can see cards
        buttonImg.sprite = fruitSprites[8];
        button.interactable = true; // reactivates buttons if not a match
    }
    #endregion

    #region GameplayMethods
    public void OnCardClick(Button button) 
    {        
        string buttonString = button.name;
        int buttonInt = (int) Int64.Parse(buttonString); // converts button name string to int
        Debug.Log(buttonInt + " was clicked!");

        FruitCard flippedCard = ScriptableObject.CreateInstance<FruitCard>();
        flippedCard = cardList[buttonInt];
        flippedCard.isFlipped = true;
        flippedCardsList.Add(flippedCard);
        pressedButtonsList.Add(button);

        Image buttonImg = button.GetComponent<Image>();
        flipCardAnimatorController = button.GetComponent<FlipCardAnimatorController>();
        flipCardAnimatorController.SetFlippingTrue(flippedCard, buttonImg);
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
                activeButtonsCount = activeButtonsCount - 1;
                if (activeButtonsCount == 0)
                {
                    outOfMatches = true;
                    StartCoroutine(EndGame());
                }
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
        yield return new WaitForSeconds(2f);
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
                outOfLives = true;
                StartCoroutine(EndGame());
            }
        }
        else
        {
            Debug.Log("ERROR: No lives left!");
        }
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f); // let animation end, maybe add function to check after animation ends if game is over
        buttonsParentObject.SetActive(false);
        triesParentObject.SetActive(false);
        if (outOfLives)
        {
            outOfLivesText.enabled = true;
            restartButtonObject.gameObject.SetActive(true);
        }
        else if (outOfMatches)
        {
            gameWonText.enabled = true;
            taskComplete = true;
            SendTaskStatus();
        }
    }

    public void RestartGame()
    {
        activeButtonsCount = 0;
        foreach (Image item in buttonsParentObject.GetComponentsInChildren<Image>(true)) 
        {
            activeButtonsCount = activeButtonsCount + 1;
            if (!item.gameObject.activeInHierarchy)
            {
                item.sprite = fruitSprites[8];
                item.GetComponent<Button>().interactable = true;
            }
            item.gameObject.SetActive(true);
        }

        // Game State (Win or Lose) Variables
        gameWonText.enabled = false;
        outOfMatches = false;
        outOfLives = false;
        outOfLivesText.enabled = false;
        restartButtonObject.gameObject.SetActive(false);

        livesLeft = 0; // gets populated in Awake()
        livesList = new List<Image>();
        foreach (Image item in livesObject.GetComponentsInChildren<Image>()) 
        {
            livesList.Add(item);
            livesLeft = livesLeft + 1;
            item.color = Color.white;
        }

        // FruitCard variables
        fruitNameList = new List<string>() {"apple","apple","banana","banana","grapes","grapes","strawberry","strawberry","watermelon","watermelon","orange","orange","pineapple","pineapple","peach","peach"};
        cardList = new List<FruitCard>();
        flippedCardsList = new List<FruitCard>();
        pressedButtonsList = new List<Button>();

        RandomizeCards();
        buttonsParentObject.SetActive(true);
        triesParentObject.SetActive(true);
    }

    public void SendTaskStatus()
    {
        if (GameController.GetInstance() != null) {
            Debug.Log("GameController.GetInstance() is not null!");
        }
        if (taskComplete)
        {
            GameController.GetInstance().SetFruitSortingComplete();
        }
    }
    #endregion
}