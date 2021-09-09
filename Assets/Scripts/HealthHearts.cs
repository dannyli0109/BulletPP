using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHearts : MonoBehaviour
{
    public GameObject[] healthHearts;
    public Character character;
    public Transform defaultPos;
    public float xOffset;

    void Update()
    {
        float holdingXOffset = -(character.hp) * xOffset / 2;
        for (int i=0; i< healthHearts.Length; i++)
        {
            if (i < character.hp)
            {
                healthHearts[i].SetActive(true);
                healthHearts[i].transform.position = defaultPos.position + new Vector3(holdingXOffset + i * xOffset, 0, 0);
            }
            else
            {
                healthHearts[i].SetActive(false);
            }
        }
    }
}
