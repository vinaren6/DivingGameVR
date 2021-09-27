using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundAsset : MonoBehaviour
{
    private static SoundAsset _instance;
    public static SoundAsset Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate((Resources.Load("SoundAsset") as GameObject).GetComponent<SoundAsset>());
            }
            return _instance;
        }
    }

    public SoundAudioClip[] audioClips;
    [System.Serializable]
    public class SoundAudioClip
    {
        [Header("Main Audio")]
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        public AudioMixerGroup mixerGroup;
        [Range(0, 1)] public float volume = 0.4f;

        [Header("For Pitching Sound")]
        public bool useRandomPitch = false;
        public float maxPitch = 1.5f;
        public float minPitch = 0.7f;

    }

}
