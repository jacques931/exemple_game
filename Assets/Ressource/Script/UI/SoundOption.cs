using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOption : MonoBehaviour
{
    [SerializeField] private Slider backSoundSlider;
    [SerializeField] private Text backSoundTxt;
    [SerializeField] private Slider dialogueSpeedSlider;
    [SerializeField] private Text dialogueSpeedtTxt;

    [SerializeField] private Transform enabledButton;
    [SerializeField] private Transform disabledButton;

    private void Start()
    {
        // L'initilisation des valeurs sont dans monsterTeamManager
        backSoundSlider.value = PlayerPrefs.GetFloat("backsound");
        dialogueSpeedSlider.value = ConvertValueToNormalized(PlayerPrefs.GetFloat("dialogueSpeed"));
        ClickSoundEffect(PlayerPrefs.GetInt("soundEffect")==0);
    }

    private void Update()
    {
        backSoundTxt.text = backSoundSlider.value.ToString("F2");
        PlayerPrefs.SetFloat("backsound",backSoundSlider.value);
        float dialogueSpeedValue = Mathf.Lerp(0.03f, 0.003f, (dialogueSpeedSlider.value - dialogueSpeedSlider.minValue) / (dialogueSpeedSlider.maxValue - dialogueSpeedSlider.minValue));
        dialogueSpeedtTxt.text = ConvertValueToNormalized(dialogueSpeedValue).ToString("F2");
        PlayerPrefs.SetFloat("dialogueSpeed",dialogueSpeedValue);
    }

    public void ClickSoundEffect(bool enabled)
    {
        Color enabledColor = enabled ? Color.green : Color.white;
        Color disabledColor = enabled ? Color.white : Color.red;

        enabledButton.GetComponent<Image>().color = enabledColor;
        disabledButton.GetComponent<Image>().color = disabledColor;

        PlayerPrefs.SetInt("soundEffect", enabled ? 0 : 1);
    }

    private float ConvertValueToNormalized(float value)
    {
        float normalizedValue = Mathf.InverseLerp(0.03f, 0.003f, value);
        return normalizedValue;
    }

    private float ConvertNormalizedToValue(float normalizedValue)
    {
        float value = Mathf.Lerp(0.03f, 0.003f, normalizedValue);
        return value;
    }
}
