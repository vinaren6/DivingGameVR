using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public string param = "Master";

    public void UpdateSlider()
    {
        var slider = GetComponent<Slider>();
        if (slider != null)
        {
            float sliderVal = slider.value;
            float playerPref = PlayerPrefs.GetFloat(param, sliderVal);
            slider.value = playerPref;

            SetLevel(slider.value);
        }
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(param, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(param, sliderValue);
    }
}
