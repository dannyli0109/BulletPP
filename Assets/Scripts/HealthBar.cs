using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Character character;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        SetMaxValue(character.stats.maxHp.value);
        SetValue(character.hp);
        fill.color = gradient.Evaluate(character.hp / character.stats.maxHp.value);
    }

    private void Update()
    {
        SetMaxValue(character.stats.maxHp.value);
        SetValue(character.hp);
        Debug.Log(character.stats.maxHp.value);
        fill.color = gradient.Evaluate(character.hp / character.stats.maxHp.value);
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}
