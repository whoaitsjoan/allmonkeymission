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
            new Vector2(0,200),
            new Vector2(100,0),
            new Vector2(100,100),
            new Vector2 (-100,-100),
            new Vector2(200,200),
            new Vector2(100,0),
            new Vector2(100,150),
            new Vector2 (-150,-100),
             new Vector2(150,150),
            new Vector2 (-200,-200)
    };
    private Transform windowTransform;
    private GameObject instantiatedWindows;
    [SerializeField] private GameObject winWindow;
    private Canvas topmostCanvas;

   


    //now grabbing items for start of minigame/ongoing timer
    [SerializeField]private TextMeshProUGUI startText;
    private float startTimer = 3.0f;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float gameTimer = 30f;
    [SerializeField] TMP_Text gameWonText;
    [SerializeField] TMP_Text gameLostText;
    [SerializeField] Button endGameButton;
    public bool taskComplete = false;
    public bool taskFailed = false;

    [SerializeField] AudioSource winAudio;
    [SerializeField] AudioSource loseAudio;
    [SerializeField] AudioSource countdownAudio;
    [SerializeField] AudioSource startAudio;
    [SerializeField] AudioSource closePopupAudio;
    private bool soundIsPlaying = false;

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
        if (startTimer > 0)
        {     
        startTimer -= Time.unscaledDeltaTime;
        startText.text = (startTimer).ToString("0");
        StartCoroutine(PlaySound("countdown"));
        }

        if (startTimer == 0)
        { StartCoroutine(PlaySound("start")); }

        if (startTimer < 0 && numberOfWindows != 0 && gameTimer > 0)
        { 
            startText.enabled = false; 
            Time.timeScale = 1f;

            countdownText.enabled = true;
            gameTimer -= Time.unscaledDeltaTime;
            countdownText.text = (gameTimer).ToString("0");
        }
        
        
        
         

        if (numberOfWindows != 0)
       { WindowCountdown(); }

        if (numberOfWindows == 0 && !taskComplete)
        { EndGame(); }

        if (gameTimer < 0 && !taskFailed)
        { EndGame(); }

    }

    IEnumerator CreatePopups()
    {   
        //create first window before starting timer to illustrate minigame concept
        //set parameters for our popupwindow
       /* windowList[0].GetComponent<Image>().sprite = windowImages[0];
        //instantiate the pop-up window as a game object
        instantiatedWindows = Instantiate(windowList[0], spawnLocations[0], Quaternion.identity) as GameObject;
        Canvas topmostCanvas = transform.root.GetComponentInChildren<Canvas>();
        instantiatedWindows.transform.SetParent(topmostCanvas.transform, false);
        */

        //yield time lines up with start timer to wait for it to finish 
        yield return new WaitForSeconds(0.5f);

        //NOW run the loop to create all the popups as the game begins
        for (int i = 0; i < numberOfWindows; i++)
        {
        windowList[i].GetComponent<Image>().sprite = windowImages[i];
        instantiatedWindows = Instantiate(windowList[i], spawnLocations[i], Quaternion.identity) as GameObject;
        topmostCanvas = transform.root.GetComponentInChildren<Canvas>();
        instantiatedWindows.transform.SetParent(topmostCanvas.transform, false); 
        }
    }
   
    IEnumerator PlaySound (string sound)
    {
        
        if (sound == "countdown" && !soundIsPlaying)
        {
            soundIsPlaying = true;
            countdownAudio.Play(0);
            yield return new WaitForSecondsRealtime(60.0f);
        }
        else if (sound == "start" && !soundIsPlaying)
        {
            soundIsPlaying = true;
            startAudio.Play(0);
            yield return new WaitForSeconds(0.5f);
        }
        else if (sound == "close")
        {
            closePopupAudio.Play(0);
        }
        else if (sound == "win" && !soundIsPlaying)
        {
            soundIsPlaying = true;
            winAudio.Play(0);
        }
        else if (sound == "lose" && !soundIsPlaying)
        {
            soundIsPlaying = true;
            loseAudio.Play(0);
        }

        soundIsPlaying = false;

    }

    void WindowCountdown()
    {   
        
        foreach (Button button in windowList.Last().GetComponentsInChildren<Button>())
            {
                button.interactable = true;
            }

    }
    

    public void ClosedPopup()
    {
        numberOfWindows--;
        windowList.Remove(windowList.Last());
        StartCoroutine(PlaySound("close"));
    }

     void EndGame()
    {
   
        if (gameTimer < 0)
        {
            taskFailed = true;
            GameObject[] windows = GameObject.FindGameObjectsWithTag("popup");
            for (int i = 0; i < windows.Length; i++)
            {
                Destroy(windows[i]);
            }
            gameLostText.enabled = true;
            endGameButton.gameObject.SetActive(true);
            StartCoroutine(PlaySound("lose"));
        }
        else if (numberOfWindows == 0)
        {
            gameWonText.enabled = true;
            Instantiate(winWindow, winWindow.transform.position, Quaternion.identity, topmostCanvas.transform);
            gameWonText.transform.SetParent(topmostCanvas.transform, false);
            StartCoroutine(PlaySound("win"));
            taskComplete = true;
            SendTaskStatus();
        }
        
    }

     public void SendTaskStatus()
    {
        if (taskComplete)
        {
            GameController.GetInstance().SetPopupComplete();
            ShipAnimationController.GetInstance().SetMiniGameComplete();
        }
    }

    

}
