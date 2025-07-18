using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer myMixer;  // Same mixer as knob
    public VolumeType volumeType = VolumeType.BGM;  // Same enum!

    public Slider slider;
    private VolumeKnob volumeKnob;

    private string ExposedParamName => volumeType.ToString();

    private float minVolume = 0.0001f;
    private float maxVolume = 1f;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        float savedVolume = PlayerPrefs.GetFloat(ExposedParamName + "Volume", 0.5f);
        slider.value = savedVolume;

        SetVolume(savedVolume);

        slider.onValueChanged.AddListener(SetVolume);
    }

    void Update()
    {

    }

    private void SetVolume(float volume01)
    {
        volume01 = Mathf.Clamp(volume01, minVolume, maxVolume);
        myMixer.SetFloat(ExposedParamName, Mathf.Log10(volume01) * 20f);
        PlayerPrefs.SetFloat(ExposedParamName + "Volume", volume01);
    }
}
