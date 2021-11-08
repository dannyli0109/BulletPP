using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Augments/CircuitSeeker")]

public class CircuitSeeker_Data : AugmentData
{

    public float homingRadiusOfNextShot;

    public override Augment Create()
    {
        return new CircuitSeeker(this);
 
    }

}