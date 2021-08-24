using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
