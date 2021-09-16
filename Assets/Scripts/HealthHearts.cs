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

    public Color damageColour;

    public Color currentColour;

    public float colourReduceAmount;


  void Start()
    {
        LastKnownHealth = character.hp;
    }

    void Update()
    {
        if(LastKnownHealth > character.hp)
        {
            currentColour = damageColour;
        }

        if(currentColour!= new Color(255, 255, 255))
        {
          //  Debug.Log("decrease");
            currentColour.b += Time.deltaTime * colourReduceAmount;
            currentColour.g += Time.deltaTime * colourReduceAmount;
            currentColour.r += Time.deltaTime * colourReduceAmount;
           // currentColour.a = 255;
        }

        float holdingXOffset = -(character.hp) * xOffset / 2;
        for (int i = 0; i< healthHearts.Length; i++)
        {
            if (i < character.hp)
            {
                healthHearts[i].gameObject.SetActive(true);
                healthHearts[i].color = currentColour;
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
