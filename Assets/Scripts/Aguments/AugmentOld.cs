using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentOld
{
    public int id;
    public int count;

    public int level
    {
        get
        {
            int value;
            if (count < 3)
            {
                value = 0;
            }
            else if (count < 9)
            {
                value = 1;
            }
            else
            {
                value = 2;
            }
            return value;
        }
    }
}

public class Synergy
{
    public int id;
    public int count;
    public List<int> breakPoints;

    public int breakPoint
    {
        get
        {
            for (int i = breakPoints.Count - 1; i >= 0; i--)
            {
                if (count >= breakPoints[i]) return i;
            }
            return -1;
        }
    }
}