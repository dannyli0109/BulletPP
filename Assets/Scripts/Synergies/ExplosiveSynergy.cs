using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplosiveSynergy : Synergy
{
    public ExplosiveSynergy(SynergyData data) : base(data)
    {
        onApply = new List<Action<Character>>();
        onApply.Add((Character character) =>
        {
            for (int i = 0; i < character.inventory.Count; i++)
            {
                for (int j = 0; j < character.inventory[i].synergies.Count; j++)
                {
                    if (character.inventory[i].synergies[j].id == id)
                    {
                        character.inventory[i].tempStatMultipliers.explosiveRadius += 0.5f;
                        character.inventory[i].tempStatMultipliers.damage += 0.5f;
                    }
                }
            }
        });

        onApply.Add((Character character) =>
        {
            for (int i = 0; i < character.inventory.Count; i++)
            {
                for (int j = 0; j < character.inventory[i].synergies.Count; j++)
                {
                    if (character.inventory[i].synergies[j].id == id)
                    {
                        character.inventory[i].tempStatMultipliers.explosiveRadius += 0.5f;
                        character.inventory[i].tempStatMultipliers.damage += 0.5f;
                    }
                }
            }
        });

        onUpdate = new List<Action<Character>>();
        onUpdate.Add((Character character) =>
        {

        });

        onUpdate.Add((Character character) =>
        {

        });
    }

}
