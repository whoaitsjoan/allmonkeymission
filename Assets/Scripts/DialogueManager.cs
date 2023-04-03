using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Parameters")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
//grabbing the dialogue panel and the text from the UI
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;



    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

//Story is from Ink referencing the Ink JSON that's going to be fed to the dialogue manager
    public Story currentStory;

//setting up different booleans here to check for the state of the dialogue typing,
//and if the player is allowed to move to the next line/skip the text typing out
    public bool isDialoguePlaying { get; private set;}
    private bool canDisplayNextLine = false;
    private bool canSkipText = false;
    private bool textSkipped = false;

    private Coroutine displayLineCoroutine;


// instance is setting this class up as a singleton that can be easily called/referenced 
// by other scripts
    private static DialogueManager instance;

//Setting up constants here that define who's speaking, what image to use for that dialogue,
//and where on the panel the image should be displaying    

private const string SPEAKER_TAG = "speaker";
private const string PORTRAIT_TAG = "portrait";
private const string LAYOUT_TAG = "layout";
private const string AUDIO_TAG = "audio";

[Header ("Audio")]
[SerializeField] private DialogueAudioProfileSO defaultAudioProfile;
[SerializeField] private DialogueAudioProfileSO[] audioProfiles;
private AudioSource audioSource;
private DialogueAudioProfileSO currentAudioProfile;
//the dictionary here is going to keep track of the ID of each speaker audio profile
//so we know who's speaking and what audio profile to use when that character is speaking
private Dictionary<string, DialogueAudioProfileSO> audioProfileDictionary;

    private void Awake()
    {
        //this is a singleton script, by design there shouldn't be more than one,
        //hence the error message
        if (instance != null)
        {
            Debug.LogError("There should only be one active version of this script!");
        }
        instance = this;
        currentAudioProfile = defaultAudioProfile;

        //setting up the audioSource to assign to the object the DialogueManager is attached to
        //as soon as the game starts running
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        //makes sure dialoguepanel is turned off and sets dialogue check to false
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        InitailizeAudioProfileDictionary();
    }

    private void Update()
    {
        //this sets a confirmation separate from ContinueStory for
        //specifically if Space is pressed while dialogue is typing
        if (Input.GetKeyDown(KeyCode.Space))
        { textSkipped = true; }

        //should return if no dialogue is playing
        if (!isDialoguePlaying)
        { return; }

        if (canDisplayNextLine && Input.GetKeyDown(KeyCode.Space))
        { ContinueStory(); }

    }
    private void InitailizeAudioProfileDictionary()
    {
        audioProfileDictionary = new Dictionary<string, DialogueAudioProfileSO>();
        audioProfileDictionary.Add(defaultAudioProfile.id, defaultAudioProfile);
        
        foreach (DialogueAudioProfileSO audioProfile in audioProfiles)
        {
            audioProfileDictionary.Add(audioProfile.id, audioProfile);
        }
    }

    private void SetCurrentAudioProfile(string id)
    {
        DialogueAudioProfileSO audioProfile = null;
        audioProfileDictionary.TryGetValue(id, out audioProfile);
        if (audioProfile != null)
        {
            this.currentAudioProfile = audioProfile;
        }
        else
        {
            Debug.LogWarning ("Could not find audio info for ID: " + id);
        }
    }

    public void EnterDialogueMode (TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);

    //before calling any new dialogue, let's make sure no info
    //from other dialogue is carrying over by setting defaults
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");


    //setting up a dedicated function to easily call across this class 
    //for checking the next line of dialogue!
       ContinueStory();
    }

    private IEnumerator ExitDialogueMode ()
    {
        yield return new WaitForSeconds(0.2f);

        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        SetCurrentAudioProfile(defaultAudioProfile.id);
    }

    private void ContinueStory()
    {
         if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            { StopCoroutine(displayLineCoroutine); }

            string nextLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine) );
            //calling a separate function here so we can handle our specific dialogue tags
            
        }
        else
        {
            //if the JSON file we're receiving is blank, we should stop the dialogue from running
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void PlayDialogueSound (int currentDisplayedCharacters, char currentCharacter)
    {
        AudioClip[] dialogueTypingSounds = currentAudioProfile.dialogueTypingSounds;
        int frequencyLevel = currentAudioProfile.frequencyLevel;
        float minPitch = currentAudioProfile.minPitch;
        float maxPitch = currentAudioProfile.maxPitch;
        bool stopAudioSource = currentAudioProfile.stopAudioSource;

        //for generating predictable audio
        //lots of math and hash configuring!

        #region predictableAudio
        AudioClip soundClip = null;
        int hashCode = currentCharacter.GetHashCode();
        int predicableIndex = hashCode % dialogueTypingSounds.Length;
        soundClip = dialogueTypingSounds[predicableIndex];

        int minPitchInt = (int) (minPitch * 100);
        int maxPitchInt = (int) (maxPitch * 100);
        int pitchRange = maxPitchInt - minPitchInt;

        if (pitchRange != 0)
        {
            int predictablePitchInt = (hashCode % pitchRange) + minPitchInt;
            float predictablePitch = predictablePitchInt / 100f;
            audioSource.pitch = predictablePitch;
        }
        else
        {
            audioSource.pitch = minPitch;
        }
        #endregion predictableAudio


        if (currentDisplayedCharacters % frequencyLevel == 0)
        {

         if (stopAudioSource)
            { audioSource.Stop(); }
            audioSource.PlayOneShot(soundClip);

        }
    }

    private IEnumerator DisplayLine (string line)
    {
        //emptying out any other existing dialogue before printing a new line,
        //along with making sure the player doesn't run two of the coroutine at once
        //and hiding the continue arrow!
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        canDisplayNextLine = false;
        continueIcon.SetActive(false);
        textSkipped = false;

        //this new boolean will be used to confirm if the ink script is passing
        //any rich text tags like colors, bold, etc
        bool isAddingRichTextTag = false;

        //starts the coroutine to space out button presses
        //so lines don't fill by mistake after a skip
        StartCoroutine(CanSkip());

        //each character is typed out at the set typing speed here
        foreach (char letter  in line.ToCharArray())
        {
            //UNLESS the player gets bored and wants to skip by hitting confirm
            if (canSkipText && textSkipped)
            {
                textSkipped = false;
                dialogueText.text = line; 
                break;
            }
            //the if check here makes sure to check if there's a rich text tag
            //and toggle true if there is and set to false if we've hit the close arrow
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                { isAddingRichTextTag = false; }
            }
            //otherwise text types out normally
            else
            {
            PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
            }

            
        }

        canDisplayNextLine = true;
        continueIcon.SetActive(true);
        canSkipText = false;
    }

    private IEnumerator CanSkip()
    {
        canSkipText = false;
        yield return new WaitForSeconds(0.05f);
        canSkipText = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        //each tag is going to get parsed here until we're finished reading anything new for this line of dialogue!
        foreach (string tag in currentTags)
        {
            //this array separates the tag out into the key and the value for each key, there should only be one of each
            string[] splitTag = tag.Split(':'); 
            if (splitTag.Length !=2)
            { Debug.LogError("Tag is not properly formatted! You wrote: " + tag); }
           string tagKey = splitTag[0].Trim();
           string tagValue = splitTag[1].Trim();  

            //the switch statement checks the Key to see if it's specifying speaker, portrait or layout for each line, if not all of them
            switch (tagKey)
            {
            case SPEAKER_TAG:
            displayNameText.text = tagValue;
            break;
            
            case PORTRAIT_TAG:
            portraitAnimator.Play(tagValue);
            break;

            case LAYOUT_TAG:
            layoutAnimator.Play(tagValue);
            break;

            case AUDIO_TAG:
            SetCurrentAudioProfile(tagValue);
            break;

            default:
            Debug.LogWarning("This tag is not currently in use: " + tag);
            break;
            }
        }

       
    }

}
