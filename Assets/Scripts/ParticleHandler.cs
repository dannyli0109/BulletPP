using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleHandler : PooledItem
{
    // Start is called before the first frame update
    VisualEffect vfx;
    bool alive = false;
    public float delay;

    void Start()
    {
        //vfx = GetComponent<VisualEffect>();
        //Destroy(gameObject, delay);
        //StartCoroutine(DeleteParticle());
    }

    public void Init()
    {
        StartCoroutine(DeleteParticle());
    }

    // Update is called once per frame
    void Update()
    {
        //if (vfx.aliveParticleCount > 0)
        //{
        //    alive = true;
        //}
        //if (alive && vfx.aliveParticleCount == 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    IEnumerator DeleteParticle()
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }
}
