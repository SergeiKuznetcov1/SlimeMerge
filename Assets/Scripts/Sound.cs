using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Sound : MonoBehaviour
{
    public AudioClip[] BGMusic;
    public AudioSource AudioSource;
    public Sprite[] SoundIcons;
    public Image BtnImage;
    private bool _musicOn = true; 
    private void Start() {
        AudioSource.clip = BGMusic[Random.Range(0, 2)];
        AudioSource.Play();
    }

    public void TurnOffSound() {
        AudioSource.volume = 0;
        BtnImage.sprite = SoundIcons[0];
    }

    public void TurnOnSound() {
        AudioSource.volume = 0.5f;
        BtnImage.sprite = SoundIcons[1];
    }

    public void SwitchMusic() {
        if (_musicOn) {
            TurnOffSound();
            _musicOn = false;
        }
        else {
            TurnOnSound();
            _musicOn = true;
        }
    }
}
