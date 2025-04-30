using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class VolumeData
{
    public float musicVolume = 0.5f;
    public float effectVolume = 0.5f;
}

public class VolumeSettings : MonoBehaviour
{
    public Slider EffectSlider;
    public Slider MusicSlider;
    public AudioSource musicSource;
    private string filePath => Path.Combine(Application.persistentDataPath, "volume_settings.json");

    void Start()
    {
        LoadVolumes();
        ApplySliders();
        // Добавим слушателей для обновления и сохранения при изменении
        EffectSlider.onValueChanged.AddListener(delegate { OnEffectVolumeChanged(); });
        MusicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
    }

    void LoadVolumes()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            VolumeData data = JsonUtility.FromJson<VolumeData>(json);
            SoundsVolume.effectVolume = data.effectVolume;
            SoundsVolume.backMusicVolume = data.musicVolume;
        }
        else
        {
            SoundsVolume.effectVolume = 0.5f;
            SoundsVolume.backMusicVolume = 0.5f;
        }
    }

    void ApplySliders()
    {
        EffectSlider.value = SoundsVolume.effectVolume;
        MusicSlider.value = SoundsVolume.backMusicVolume;
    }

    void SaveVolumes()
    {
        VolumeData data = new VolumeData
        {
            effectVolume = EffectSlider.value,
            musicVolume = MusicSlider.value
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    void OnEffectVolumeChanged()
    {
        SoundsVolume.effectVolume = EffectSlider.value;
        SaveVolumes();
    }

    void OnMusicVolumeChanged()
    {
        SoundsVolume.backMusicVolume = MusicSlider.value;
        musicSource.volume = SoundsVolume.backMusicVolume;
        SaveVolumes();
    }
}
