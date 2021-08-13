using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    public Slider slider;
    public Player character;
    public Gradient gradient;
    public Image fill;
    public GameObject reloadBar;

    private void Start()
    {
        UpdateValues();
    }

    private void Update()
    {
        if (character.reloading)
        {
            reloadBar.SetActive(true);
            UpdateValues();
        }
        else
        {
            reloadBar.SetActive(false);
        }
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }


    private void UpdateValues()
    {
        float currentReloadTime = character.currentReloadTime;
        float maxReloadTime = character.stats.reloadTime.value;

        SetMaxValue(maxReloadTime);
        SetValue(currentReloadTime);

        fill.color = gradient.Evaluate(currentReloadTime / maxReloadTime);
    }
}
