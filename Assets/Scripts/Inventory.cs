﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int capacity = 3;
    public List<Augment> augments;

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        augments = new List<Augment>();
    }

    public bool AddTo(Augment augment)
    {
        if (augments.Count >= capacity) return false;
        augments.Add(augment);
        return true;
    }

    public bool RemoveAt(int index)
    {
        if (index >= augments.Count) return false;
        if (augments.Count == 1) return false;
        augments.RemoveAt(index);
        return true;
    }
}