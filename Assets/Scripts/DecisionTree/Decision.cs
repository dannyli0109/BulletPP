using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Decision
{
    public Action action;
    public Func<bool> condition;
    public Decision trueBranch;
    public Decision falseBranch;

    public void MakeDecision()
    {
        action();
        if (condition != null && condition())
        {
            if (trueBranch == null) return;
            trueBranch.MakeDecision();
        }
        else
        {
            if (falseBranch == null) return;
            falseBranch.MakeDecision();
        }
    }
}
