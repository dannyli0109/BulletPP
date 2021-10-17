using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public SoundType soundType;
    public void LeftFootStepCallback(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5)
        {
            SoundManager.PlaySound(soundType, leftFoot.position, 1);
        }
    }

    public void rightFootStepCallback(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5)
        {
            SoundManager.PlaySound(soundType, rightFoot.position, 1);
        }
    }
}
