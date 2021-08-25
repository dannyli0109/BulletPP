﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy")]
public class SynergyData : ScriptableObject
{
    public int id;
    public string title;

    public List<int> breakpoints;

    [TextArea(5, 20)]
    public List<string> descriptions;

    [TextArea(5, 20)]
    public List<string> codes;

    public List<Eva> evaluators;


    void Reset()
    {
        //Output the message to the Console
        id = Resources.LoadAll("Data/Synergies", typeof(SynergyData)).Length;
    }

}
