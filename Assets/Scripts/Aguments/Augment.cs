using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Augment
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
