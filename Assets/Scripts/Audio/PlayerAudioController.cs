using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] AudioSource walkingAudio;

    public void PlayWalkingAudio()
    {
        walkingAudio.Play(0);
    }
}
