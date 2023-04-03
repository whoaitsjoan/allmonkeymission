using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InstructionsScreen_PlayVideo : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] Texture rawImgTexture;
    [SerializeField] VideoPlayer videoPlayer;

    private void OnEnable()
    {
        rawImage.texture = rawImgTexture;
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
