using UnityEngine;

public class PlayBackGroundMusic : Sounds
{

    void Start()
    {
        PlayMusic(sounds[0],SoundsVolume.backMusicVolume);
    }

}
