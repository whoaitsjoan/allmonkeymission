using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupGameController : MonoBehaviour
{
    //game variables/objects needed for main minigame function first
   

    //next the popupwindow specifics
    [SerializeField]private List<GameObject> windowList;
    [Range (1,10)]
    [SerializeField] private int numberOfWindows;
    [SerializeField] Sprite[] windowImages;
    List<Vector2> spawnLocations = new List<Vector2>(){
            new Vector2(0,5),
            new Vector2(5,0),
            new Vector2(5,5),
            new Vector2 (-4,-4)
    };
    private Transform windowTransform;


    //now grabbing items for start of minigame/ongoing timer
    [SerializeField]private TextMeshProUGUI startText;
    private float startTimer = 3.0f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float gameTimer = 30f;
    public bool taskComplete = false;

    //setting up class as a singleton class
    private static PopupGameController instance;

    void Awake() 
    {


    if (instance != null)
        {
            Debug.LogError("There should only be one active version of this script!");
        }
        instance = this;

        //now we can call our co-routine to prepare the popup windows
        StartCoroutine(CreatePopups());

    }

    // Start is called before the first frame update
    void Start()
    {
        //setting time to 0 so that
        //only the start timer is running in the beginning
        if (startTimer != 0)
        { Time.timeScale = 0; }
        

        
        
    }

    // Update is called once per frame
    void Update()
    {
        //starts a countdown in the start text      
        startTimer -= Time.unscaledDeltaTime;
        startText.text = (startTimer).ToString("0");

        if (startTimer < 0)
        { 
            startText.enabled = false; 
            Time.timeScale = 1f;
            countdownText.enabled = true;
        }
        
        
        
         gameTimer -= Time.unscaledDeltaTime;
        countdownText.text = (gameTimer).ToString("0");

        if (numberOfWindows != 0)
       { WindowCountdown(); }

        if (numberOfWindows == 0)
        { SendTaskStatus(); }


    }

    IEnumerator CreatePopups()
    {   
        //create first window before starting timer to illustrate minigame concept
        //set parameters for our popupwindow
        windowList[0].GetComponent<Image>().sprite = windowImages[0];
        //instantiate the pop-up window as a game object
        Instantiate(windowList[0], spawnLocations[0], Quaternion.identity);
        //and finally add the scriptableobject
        //as a component of the gameobject

        //yield time lines up with start timer to wait for it to finish 
        yield return new WaitForSeconds(startTimer);

        //NOW run the loop to create all the popups as the game begins
        for (int i = 1; i < numberOfWindows; i++)
        {
        windowList[i].GetComponent<Image>().sprite = windowImages[i];
        Instantiate(windowList[i], spawnLocations[i], Quaternion.identity);
        }
    }
    

    void WindowCountdown()
    {   
        iTween.ShakePosition(windowList.Last(), new Vector3 (1,1,0), 5f);
        foreach (Button button in windowList.Last().GetComponentsInChildren<Button>())
            {
                button.interactable = true;
            }

    }
    

    void ClosedPopup()
    {
        numberOfWindows--;
        windowList.Remove(windowList.Last());
    }

     public void SendTaskStatus()
    {
        if (GameController.GetInstance() != null) {
            Debug.Log("GameController.GetInstance() is not null!");
        }
        if (taskComplete)
        {
            GameController.GetInstance().SetPopupComplete();
        }
    }

}
