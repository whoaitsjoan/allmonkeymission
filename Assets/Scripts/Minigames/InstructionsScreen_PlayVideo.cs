using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstructionsScreen_PlayVideo : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] VideoPlayer videoPlayer;

    void Awake()
    {
        // rawImage.GetComponent<RawImage>();
        // videoPlayer.GetComponent<VideoPlayer>();
        StartCoroutine(PlayVideo());
    }

    private IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(.2f);
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

}
