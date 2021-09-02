﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Augment")]
public class AugmentData : ScriptableObject
{
    public int id;
    public string title;

    public int rarity;

    public List<SynergyData> synergies;

    public string iconPath;

    [TextArea(5, 20)]
    public List<string> descriptions;

    [TextArea(5, 20)]
    public List<string> codes;

    public List<Eva> evaluators;

    public Sprite iconSprite;

    void Reset()
    {
        //Output the message to the Console
        id = Resources.LoadAll("Data/Augments", typeof(AugmentData)).Length;
    }

}