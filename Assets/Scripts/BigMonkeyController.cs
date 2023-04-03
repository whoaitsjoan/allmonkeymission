using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigMonkeyController : MonoBehaviour
{
    [SerializeField] AudioSource monkeAudio;
    [SerializeField] GameObject visualCue;

    public string sceneToLoad;

    bool playerInRange = false;

    void Update()
    {
        if (playerInRange)
        {
            StartCoroutine(PlayMonkeSound());
        }
    }
    
    // private void OnEnable()
    // {
    //     monkeAudio.Play(0);
    // }

    private void OnTriggerEnter2D(Collider2D col)
    {
        visualCue.SetActive(true);
        if (col.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            visualCue.SetActive(false);
            playerInRange = false;
        }
    }

    private IEnumerator PlayMonkeSound()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            monkeAudio.Play(0);

            yield return new WaitForSeconds(2f);
            RollCredits();
        }
    }

    private void RollCredits()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
