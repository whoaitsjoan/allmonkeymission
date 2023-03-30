using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    private bool playerInRange = false;

    private GameController gameControllerInstance;

    [SerializeField] private GameObject fruitSortingInstructions;
    [SerializeField] private GameObject popupInstructions;


    private static EnableMiniGame instance;

    // Start is called before the first frame update
    void Start()
    {
        visualCue.SetActive(false);
        gameControllerInstance = GameController.GetInstance();

        if (instance != null)
        {
            Debug.Log("ERROR: more than one PlayerController in scene!");
        }
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.transform.parent.name == "Refrigerator" && !GameController.GetInstance().GetFruitSortingStatus())
        {
            if (playerInRange)
            {
                visualCue.SetActive(true);
            }
        }
        else if (this.transform.parent.name == "Monitors" && !GameController.GetInstance().GetPopupStatus())
        {
            if (playerInRange)
            {
                visualCue.SetActive(true);
            }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    public bool GetPlayerInRange()
    {
        return playerInRange;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public void OpenInstructionsScreen()
    {
        Debug.Log("entered open instructions screen function");
        Debug.Log("this.gameObject.name is " + this.gameObject.name);
        if (this.transform.parent.name == "Refrigerator")
        {
            fruitSortingInstructions.SetActive(true);
        }
        else if (this.transform.parent.name == "Monitors")
        {
            popupInstructions.SetActive(true);
        }
    }

    public static EnableMiniGame GetInstance()
    {
        return instance;
    }
}
