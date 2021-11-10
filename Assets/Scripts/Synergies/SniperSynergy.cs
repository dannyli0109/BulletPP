using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SniperSynergy 
    : Synergy
{
    public SniperSynergy(SynergyData data) : base(data)
    {
        onApply = new List<Action<Character>>();
        onApply.Add((Character character) =>
        {
        });

        onApply.Add((Character character) =>
        {
            for (int i = 0; i < character.inventory.Count; i++)
            {
                for (int j = 0; j < character.inventory[i].synergies.Count; j++)
                {
                    if (character.inventory[i].synergies[j].id == id)
                    {
                        character.inventory[i].tempStats.homingRadius += 30.0f;                   
                    }
                }
            }
        });


        onUpdate = new List<Action<Character>>();
        onUpdate.Add((Character character) =>
        {
            LaserSight(character);
        });

        onUpdate.Add((Character character) =>
        {
            LaserSight(character);
        });

    }

    void LaserSight(Character character)
    {
        LineRenderer lineRenderer = character.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        Vector3 lookDir = character.bulletContainer.forward * 100;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, character.bulletContainer.position);
        lineRenderer.SetPosition(1, character.bulletContainer.position + lookDir);
    }
        
}
