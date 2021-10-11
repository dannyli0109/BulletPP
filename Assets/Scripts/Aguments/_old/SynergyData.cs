using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "New Synergy")]
public class SynergyData : ScriptableObject
{
    public int id;
    public string title;

    public List<int> breakpoints;

    public string iconPath;

    [TextArea(5, 20)]
    public List<string> descriptions;

    [TextArea(5, 20)]
    public List<string> codes;

    public UnityEvent OnAttached;
    public UnityEvent OnUpdate;

    public List<Eva> evaluators;

    public Sprite iconSprite;

    void Reset()
    {
        //Output the message to the Console
        id = Resources.LoadAll("Data/Synergies", typeof(SynergyData)).Length;
    }

}
