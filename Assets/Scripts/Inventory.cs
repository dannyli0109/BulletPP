using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int capacity;
    public List<Augment> augments;

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        augments = new List<Augment>();
    }

    public Augment this[int index]
    {
        get => augments[index];
        set
        {
            augments[index] = value;
        }
    }

    public int Count
    {
        get => augments.Count;
    }

    public bool AddTo(Augment augment, Character character)
    {
        if (augments.Count >= capacity) return false;
        augments.Add(augment);
        augments[augments.Count - 1].OnAttached(character, augments.Count - 1);

        
        if (FindAugment(augment, augments.Count - 1) == -1)
        {
            for (int i = 0; i < augments[augments.Count - 1].synergies.Count; i++)
            {
                character.AddSynergy(augments[augments.Count - 1].synergies[i].Create());
            }
        }
        character.UpdateSynergies();

        //character.ApplyS
        
        // find if the player have that augment already
        // if no, add the synergies to the player
        // apply the new synergy

        return true;
    }

    public int FindAugment(Augment augment, int index)
    {
        for (int i = 0; i < augments.Count; i++)
        {
            if (augments[i].id == augment.id && i != index) return i;
        }
        return -1;
    }

    public bool RemoveAt(int index)
    {
        if (index >= augments.Count) return false;
        augments.RemoveAt(index);
        return true;
    }
}
