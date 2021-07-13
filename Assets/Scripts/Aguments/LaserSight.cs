using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : Augment
{
    public List<StatModifierData> modifierDatas;
    public Transform gunPoint;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < modifierDatas.Count; i++)
        {
            StatModifierData data = modifierDatas[i];
            StatModifier modifier = new StatModifier(data.amounts, data.modifierType);
            character.AddModifier(data.type, data.stat, modifier);
        }
    }

    private void Update()
    {
        Vector3 lookDir = gunPoint.forward * 100;
        lineRenderer.SetPosition(0, gunPoint.position);
        lineRenderer.SetPosition(1, gunPoint.position + lookDir);
    }

}
