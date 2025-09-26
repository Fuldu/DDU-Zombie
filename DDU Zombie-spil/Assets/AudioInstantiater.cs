using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstantiater : MonoBehaviour
{

    public GameObject AudioObjectPre;


    public void InstantiateAudio(AudioClip audio, Vector2 pos)
    {
        GameObject newAudioObject = Instantiate(AudioObjectPre, pos, Quaternion.identity);

        newAudioObject.GetComponent<AudioSource>().clip = audio;
        newAudioObject.GetComponent<AudioSource>().Play();
    }




}
