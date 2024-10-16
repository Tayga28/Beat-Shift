using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    //public AudioClip track;
    public AudioSource audio;
    public PlayerMovement player;
    public bool musicIsPlaying;
    // Start is called before the first frame update
    void Start()
    {
        //audio = GetComponent<AudioSource>();
        musicIsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(player.userOnMenu) return;
        if(!musicIsPlaying && player.madeFirstContactWithLevel)
        {
            musicIsPlaying = true;
            audio.Play();
            audio.volume = 1f;
        } else if (player.userOnMenu)
        {
            audio.Stop();
            musicIsPlaying = false;
        }
    }
}
