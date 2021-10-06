using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamagable
{
    void OnDamage(IDamageDealer source);
}

public interface IHittable
{
    void OnHit(IDamageDealer source);
}

public interface IDamageDealer
{
    float GetDamage();
}

public interface IBouncable
{
    void Bounce(Collision collision);
}
