using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMiniGame : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    private bool playerInRange = false;

    [SerializeField] private GameObject fruitSortingInstructions;
    [SerializeField] private GameObject popupInstructions;


    // Start is called before the first frame update
    void Start()
    {
        visualCue.SetActive(false);

       
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

        if (Input.GetKey(KeyCode.Return) && playerInRange)
        {
             Debug.Log("Getting OpenInstructionsScreen()");
            OpenInstructionsScreen();
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
            PlayerController.GetInstance().PlayingMinigame();
        }
        else if (this.transform.parent.name == "Monitors")
        {
            popupInstructions.SetActive(true);
            PlayerController.GetInstance().PlayingMinigame();
        }
    }

    
}
