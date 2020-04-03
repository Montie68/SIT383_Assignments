using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // audio source to play
    public AudioSource audioSource;
    Canvas canvas;
    bool hasPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        // Get audio source if not set
        if (audioSource == null) audioSource = this.GetComponent<AudioSource>();
        canvas = this.GetComponent<Canvas>();
    }
    // Play sound on enable
    
    void Update()
    {
        // test if canvas is enabled recently an play the sound.
        if (canvas.enabled == true && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true;
        }
        else if (canvas.enabled == false && hasPlayed)
        {
            audioSource.Stop();
            hasPlayed = false;
        }
    }

}
