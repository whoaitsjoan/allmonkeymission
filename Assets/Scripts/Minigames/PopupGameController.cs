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
   // public GameObject livesObject;
    private List<Image> livesList = new List<Image>();
    private List<PopupWindow> windowList = new List<PopupWindow>();

    //now grabbing items for start of minigame/ongoing timer
    [SerializeField]private TextMeshProUGUI startText;
    private float startTimer = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        startTimer -= Time.deltaTime;
        startText.text = (startTimer).ToString("0");

    }
}
