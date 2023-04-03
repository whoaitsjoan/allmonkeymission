using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DialogueAudioProfileSO", menuName = "ScriptableObjects/DialogueAudioProfileSO", order = 1)]
public class DialogueAudioProfileSO : ScriptableObject
{
    public string id;

    public AudioClip[] dialogueTypingSounds;
    public bool stopAudioSource;
    [Range(1,5)]
    public int frequencyLevel = 2;
    [Range(-3,3)]
    public float minPitch = 0.5f;
    [Range(-3,3)]
    public float maxPitch = 3f;

}
