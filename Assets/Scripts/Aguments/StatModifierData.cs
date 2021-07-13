using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatModifierData : ScriptableObject
{
    public StatType type;
    public string stat;
    public float amounts;
    public StatModType modifierType;
}
