using System.Collections;
using UnityEngine;

public class PlayBackGroundMusic : Sounds
{

    IEnumerator Start()
    {
        yield return null; // ��� ���� ����
        PlayMusic(sounds[0], SoundsVolume.backMusicVolume);
    }

}
