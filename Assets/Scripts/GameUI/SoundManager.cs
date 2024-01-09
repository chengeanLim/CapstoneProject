using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] AudioClips;

    [SerializeField] AudioSource BGMPlayer; // ��� ����
    [SerializeField] AudioSource SFXPlayer; // ȿ����
    [SerializeField] Slider SoundSlider;

    void Awake()
    {
        SoundSlider.onValueChanged.AddListener(ChangeSoundVolume);
    }
    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "ButtonClick": index = 0; break; // ��ư Ŭ�� ȿ����
            case "Enhance": index = 1; break; // ��ȭ ȿ����
        }

        SFXPlayer.clip = AudioClips[index];
        SFXPlayer.Play();
    }

    void ChangeSoundVolume(float value)
    {
        BGMPlayer.volume = value;
        SFXPlayer.volume = value;
    }
}
