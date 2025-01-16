using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Sliders")]
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;

    [Header("Text Fields")]
    public TextMeshProUGUI xSensitivityText;
    public TextMeshProUGUI ySensitivityText;

    private void Awake()
    {
        PlayerPrefs.SetFloat("xSensitivity", 30f);
        PlayerPrefs.SetFloat("ySensitivity", 30f);
        PlayerPrefs.Save();
    }

    private void OnEnable()
    {
        xSensitivitySlider.value = PlayerPrefs.GetFloat("xSensitivity", 30f);
        ySensitivitySlider.value = PlayerPrefs.GetFloat("ySensitivity", 30f);

        UpdateXSensitivityText(xSensitivitySlider.value);
        UpdateYSensitivityText(ySensitivitySlider.value);
    }

    private void Start()
    {
        xSensitivitySlider.onValueChanged.AddListener(SetXSensitivity);
        ySensitivitySlider.onValueChanged.AddListener(SetYSensitivity);
    }

    public void SetXSensitivity(float newSensitivity)
    {
        PlayerPrefs.SetFloat("xSensitivity", newSensitivity);

        UpdateXSensitivityText(newSensitivity);
    }

    public void SetYSensitivity(float newSensitivity)
    {
        PlayerPrefs.SetFloat("ySensitivity", newSensitivity);

        UpdateYSensitivityText(newSensitivity);
    }

    private void UpdateXSensitivityText(float value)
    {
        xSensitivityText.text = value.ToString();
    }

    private void UpdateYSensitivityText(float value)
    {
        ySensitivityText.text = value.ToString();
    }
}
