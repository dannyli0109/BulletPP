using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Augment")]
public class AugmentData : ScriptableObject
{
    public int id;
    public string title;

    public int rarity;

    public List<SynergyData> synergies;

    [TextArea(5, 20)]
    public List<string> descriptions;

    [TextArea(5, 20)]
    public List<string> codes;

    public List<Eva> evaluators;

}
