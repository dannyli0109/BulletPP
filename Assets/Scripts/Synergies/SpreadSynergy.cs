using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpreadSynergy : Synergy
{
    public SpreadSynergy(SynergyData data) : base(data)
    {
        onApply = new List<Action<Character>>();
        onApply.Add((Character character) =>
        {
            for (int i = 0; i < character.inventory.Count; i++)
            {
                for (int j = 0; j < character.inventory[i].synergies.Count; j++)
                {
                    if (character.inventory[i].GetAmounts(character, i) > 1)
                    {
                        character.inventory[i].tempStatMultipliers.damage += 1f;
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
                    if (character.inventory[i].GetAmounts(character, i) > 1)
                    {
                        character.inventory[i].tempStatMultipliers.damage += 2f;
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
