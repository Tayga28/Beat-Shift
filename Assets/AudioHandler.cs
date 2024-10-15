using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    //public AudioClip track;
    public AudioSource audio;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        //audio = GetComponent<AudioSource>();
        audio.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //if(player.userOnMenu) return;
        if(player.hasGameplayStarted == true)
        {
            audio.Play();
        }
    }
}
