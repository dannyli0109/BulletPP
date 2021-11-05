using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SynergyData : ScriptableObject
{
    public int id;
    public List<int> breakpoints;
    [TextArea(5, 10)]
    public List<string> descriptions;
    public Color color;

    public abstract Synergy Create();

}
