using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class SoundManager
{

    public enum Sound
    {
        ClickOne,
        ClickTwo,
        ClickThree,
        PlayerMove,
        Bubblor,
        ShootSound,
        Kaboom,
    }

    private static GameObject playOneShotGameObj;
    private static AudioSource playOneShotAudioSource;

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            //objectpoola 3d ljud?
            GameObject soundObj = new GameObject("Sound: " + sound.ToString());
            soundObj.transform.position = position;
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();

            var clip = GetAudioClip(sound);
            audioSource.clip = clip.audioClip;
            audioSource.volume = clip.volume;
            audioSource.outputAudioMixerGroup = clip.mixerGroup;

            if (clip.useRandomPitch)
                audioSource.pitch = Random.Range(clip.minPitch, clip.maxPitch);

            //else pitch = 1;
            audioSource.maxDistance = 100f;
            audioSource.spatialBlend = 1;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;
            audioSource.Play();

            Object.Destroy(soundObj, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (playOneShotGameObj == null)
            {
                playOneShotGameObj = new GameObject("Sound");
                playOneShotAudioSource = playOneShotGameObj.AddComponent<AudioSource>();
            }

            var clip = GetAudioClip(sound);
            if (clip.useRandomPitch)
                playOneShotAudioSource.pitch = Random.Range(clip.minPitch, clip.maxPitch);
            playOneShotAudioSource.outputAudioMixerGroup = clip.mixerGroup;

            //else pitch = 1;
            playOneShotAudioSource.volume = clip.volume;
            playOneShotAudioSource.PlayOneShot(clip.audioClip);
        }
    }
    private static Dictionary<Sound, float> soundTimerDictionary;
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0;
    }
    static bool didfgrdsphj = true;
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
                {
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = soundTimerDictionary[sound];
                        float playerMoveTimerMax = 10.7f;
                        if(didfgrdsphj == true)
                        {
                            didfgrdsphj = false;
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }

                        if (lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return true;
                    //break;
                }
        }
    }

    private static SoundAsset.SoundAudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAsset.SoundAudioClip audioClip in SoundAsset.Instance.audioClips)
        {
            if (sound == audioClip.sound)
            {
                return audioClip;
            }
        }
        Debug.LogError("Sound not found " + sound);
        return null;
    }
}
