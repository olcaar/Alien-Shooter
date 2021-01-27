using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour
{
   public void MuteMusic()
    {
        MusicPlayer Player = FindObjectOfType<MusicPlayer>();
        Player.GetComponent<AudioSource>().mute = !(Player.GetComponent<AudioSource>().mute);
    }
}
