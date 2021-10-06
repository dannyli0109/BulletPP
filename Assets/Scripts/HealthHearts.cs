using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHearts : MonoBehaviour
{
    public Image[] healthHearts;
    public Character character;
    public Transform defaultPos;
    public float xOffset;
    float LastKnownHealth;

    public Color HeartdamageColour;

    public Color HeartcurrentColour;

    public float colourReduceAmount;

    public Image onHurtScreenGlow;
    public Color onHurtScreenGlowColour;


  void Start()
    {
        LastKnownHealth = character.hp;
    }

    void Update()
    {
        if(LastKnownHealth > character.hp)
        {
            HeartcurrentColour = HeartdamageColour;
            onHurtScreenGlow.color = onHurtScreenGlowColour;
        }

        if(HeartcurrentColour!= new Color(255, 255, 255))
        {
          //  Debug.Log("decrease");
            HeartcurrentColour.b += Time.deltaTime * colourReduceAmount;
            HeartcurrentColour.g += Time.deltaTime * colourReduceAmount;
            HeartcurrentColour.r += Time.deltaTime * colourReduceAmount;
            HeartcurrentColour.a = 255;
        }

        if (onHurtScreenGlow.color.a > 0)
        {
            Color holdingColour = new Color(onHurtScreenGlow.color.r, onHurtScreenGlow.color.g, onHurtScreenGlow.color.b, onHurtScreenGlow.color.a - Time.deltaTime * colourReduceAmount);
            onHurtScreenGlow.color = holdingColour;
        }

        float holdingXOffset = -(character.hp) * xOffset / 2;
        for (int i = 0; i< healthHearts.Length; i++)
        {
            if (i < character.hp)
            {
                healthHearts[i].gameObject.SetActive(true);
                healthHearts[i].color = HeartcurrentColour;
                healthHearts[i].gameObject.transform.position = defaultPos.position + new Vector3(holdingXOffset + i * xOffset, 0, 0);
            }
            else
            {
                healthHearts[i].gameObject.SetActive(false);
            }
        }

        LastKnownHealth = character.hp;
    }
}
