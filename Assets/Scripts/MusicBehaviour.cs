using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBehaviour : MonoBehaviour
{
    public AudioSource track;
    public void FadeMusic()
    {
        track.volume --;
    }
}
