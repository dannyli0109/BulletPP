using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplosiveSynergy : Synergy
{
    public ExplosiveSynergy(SynergyData data) : base(data)
    {
        actions = new List<Action<Character>>();
        actions.Add((Character character) =>
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

        actions.Add((Character character) =>
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
    }

}
