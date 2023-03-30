using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupGameController : MonoBehaviour
{
    //grabbing game objects needed for main minigame function first
    private int livesLeft = 5; 
    public GameObject livesObject;
    private List<Image> livesList = new List<Image>();
    [SerializeField]private List<GameObject> windowList = new List<GameObject>();
    [Range (1,10)]
    [SerializeField] private int numberOfWindows;

    //now grabbing items for start of minigame/ongoing timer
    [SerializeField]private TextMeshProUGUI startText;
    private float startTimer = 3.0f;
    


    void Awake() 
    {
    foreach (Image item in livesObject.GetComponentsInChildren<Image>()) 
        {
            livesList.Add(item);
        }

        StartCoroutine(CreatePopups());

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //starts a countdown in the start text
        startTimer -= Time.deltaTime;
        startText.text = (startTimer).ToString("0");
        if (startTimer < 0)
        { startText.enabled = false; }

    }

    IEnumerator CreatePopups()
    {
        yield return new WaitForSeconds(startTimer);
    }
}
