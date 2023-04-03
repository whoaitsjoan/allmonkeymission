using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public DialogueManager dm;

    

    public void loadScene()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
    

    void OnDisable()
    {
        if (!dm.currentStory.canContinue)
        {
        loadScene();
        }
    }
}
