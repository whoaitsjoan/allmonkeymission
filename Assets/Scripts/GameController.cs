using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private bool fruitSortingComplete = false;
    private bool popupComplete = false;
    private bool winGame = false;

    [SerializeField] private TMP_Text fruitSortingTask;
    [SerializeField] private TMP_Text popupTask;

    public GameObject spaceBanana;
    public GameObject bigMonkey;
    public GameObject originalWall;
    public GameObject fakeWall;

    private static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ERROR: more than one GameController in scene!");
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (fruitSortingComplete && popupComplete && !winGame)
        { endOfGame(); }
    }

    public static GameController GetInstance()
    {
        return instance;
    }

    public bool GetFruitSortingStatus()
    {
        return fruitSortingComplete;
    }

    public void SetFruitSortingComplete()
    {
        fruitSortingComplete = true;
        fruitSortingTask.fontStyle = FontStyles.Bold | FontStyles.Strikethrough;
    }

    public bool GetPopupStatus()
    {
        return popupComplete;
    }

    public void SetPopupComplete()
    {
        popupComplete = true;
        popupTask.fontStyle = FontStyles.Bold | FontStyles.Strikethrough;
    }

    public void endOfGame()
    {
        winGame = true;
        spaceBanana.SetActive(true);
        bigMonkey.SetActive(true);
        originalWall.SetActive(false);
        fakeWall.SetActive(true);
    }
}
