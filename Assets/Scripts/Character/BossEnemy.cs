using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEnemy : SpellQueueEnemy
{
    public override void Start()
    {
        base.Start();
        finalDestination = target.transform.position;
        agent.acceleration = setAcceleration;
    }

    public override void Init(Player target, Transform cam, AmmoPool ammoPool)
    {
        base.Init(target, cam, ammoPool);
        InitSpellQueue();

        decision = new Decision()
        {
            action = () => { },
            condition = () =>
            {
                return InRange(tooFarToShoot);
            },
            trueBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return InLineOfSight(60);
                },
                trueBranch = ToMove(),
                falseBranch = ToMove()
            },
            falseBranch = new Decision()
            {
                action = () => { },
                condition = () =>
                {
                    return InRange(1000);
                },
                trueBranch = ToMove(),
                falseBranch = null
            }
        };
    }

    public override void InitSpellQueue()
    {
        spellQueue = new List<Action>();
        spellTime = new List<float>();

        index = 0;
    }
}
