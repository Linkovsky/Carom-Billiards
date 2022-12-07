using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace CaromBilliards.Sounds
{
    public class SoundMenu : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private AudioMixer audioMixer;

        private const string _volumeMaster = "MasterVolume";
        private float _value;
        private float _sliderValue;
        private void Start()
        {
            if (_value != 0)
            {
                volumeSlider.value = _value;
            }
            else
            {
                if (AudioManager.Instance != null && File.Exists(AudioManager.Instance.path))
                {
                    _value = AudioManager.Instance.LoadFromJSON();
                    volumeSlider.value = _value;
                }
                else
                {
                    volumeSlider.value = 1;
                }
            }
        }
        public void ChangeVolume(float sliderValue)
        {
            _sliderValue = sliderValue;
            audioMixer.SetFloat(_volumeMaster, Mathf.Log10(sliderValue) * 20); // db to linear
        }

        public void Save()
        {
            if(AudioManager.Instance != null)
                AudioManager.Instance.SaveToJSON(_sliderValue);
        }
    }
}

