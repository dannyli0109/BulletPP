using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public GameObject target;

    float angle;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!target) return;
        Vector2 current = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.z);
        angle = Util.AngleBetweenTwoPoints(targetPos, current) + 90;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

        timeSinceFired += Time.deltaTime;

        if (timeSinceFired >= bulletStats.fireRate.value)
        {
            Shoot();
            timeSinceFired = 0;
        }
    }
}
