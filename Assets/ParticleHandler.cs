using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleHandler : MonoBehaviour
{
    // Start is called before the first frame update
    VisualEffect vfx;
    bool alive = false;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vfx.aliveParticleCount > 0)
        {
            alive = true;
        }
        if (alive && vfx.aliveParticleCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
