using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    [SerializeField] private string triggerTag = "Player";
    [SerializeField] private AudioPlayer audioPlayerPrefab;

    [SerializeField] private AudioClip audioToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private float pitch = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == triggerTag)
        {
            PlayAudio();
        }
    }

    private void PlayAudio()
    {
        AudioPlayer newPlayer = Instantiate(audioPlayerPrefab);
        newPlayer.PlayAudio(audioToPlay,volume,pitch);
    }
}
