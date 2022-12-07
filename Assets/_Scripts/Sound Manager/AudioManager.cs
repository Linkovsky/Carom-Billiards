using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace CaromBilliards.Sounds
{
    public class AudioManager : MonoBehaviour
    {
        internal static AudioManager Instance;
        [SerializeField] private AudioMixer audioMixer;
    
        [HideInInspector] public string path;

        private LoadFromJSONFile _loadSoundValue;
        private const string _volumeMaster = "MasterVolume";
        private void Awake()
        {
            Instance = this;
            path = Application.persistentDataPath + "/AudioSettings.dat";
        }
    
        private void OnEnable()
        {
            if (File.Exists(path))
            {
                float value = LoadFromJSON();
                audioMixer.SetFloat(_volumeMaster, Mathf.Log10(value) * 20f);
            }
        }

        internal void SaveToJSON(float sliderValue)
        {
            _loadSoundValue.value = sliderValue;
            using FileStream fileStream = new FileStream(path, FileMode.Create);
            using StreamWriter streamWriter = new StreamWriter(fileStream);
            string save = JsonUtility.ToJson(_loadSoundValue);
            streamWriter.Write(save);
            streamWriter.Close();
            fileStream.Close();
        }
        internal float LoadFromJSON()
        {
            using FileStream fileStream = new FileStream(path, FileMode.Open);
            using StreamReader streamReader = new StreamReader(fileStream);
            _loadSoundValue = JsonUtility.FromJson<LoadFromJSONFile>(streamReader.ReadLine());
            streamReader.Close();
            fileStream.Close();
            return _loadSoundValue.value;
        }
    }
    internal struct LoadFromJSONFile
    {
        public float value;
    }
}
