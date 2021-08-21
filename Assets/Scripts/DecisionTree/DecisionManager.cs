using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionManager : MonoBehaviour
{
    private static DecisionManager current;

    private void Awake()
    {
        current = this;
    }

   

}
