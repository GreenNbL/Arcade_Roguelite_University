using System.Collections;
using UnityEngine;

public class PlayBackGroundMusic : Sounds
{

    IEnumerator Start()
    {
        yield return null; // ∆дЄм один кадр
        PlayMusic(sounds[0], SoundsVolume.backMusicVolume);
    }

}
