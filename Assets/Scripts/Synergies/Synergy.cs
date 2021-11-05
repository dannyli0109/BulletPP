using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Synergy 
{
    public int id;
    public List<int> breakpoints;
    [TextArea(5, 10)]
    public List<string> descriptions;
    public Color color;
    public List<Action<Character>> actions;
    public int count;

    public int breakPoint
    {
        get
        {
            for (int i = breakpoints.Count - 1; i >= 0; i--)
            {
                if (count >= breakpoints[i]) return i;
            }
            return -1;
        }
    }

    public Synergy(SynergyData data)
    {
        id = data.id;
        breakpoints = data.breakpoints;
        descriptions = data.descriptions;
        color = data.color;
        count = 1;
    }

    public void Apply(Character character)
    {
        int value = breakPoint;
        if (value > -1)
        {
            actions[value](character);
        }
    }
}
