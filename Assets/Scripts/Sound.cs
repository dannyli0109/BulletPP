using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : PooledItem
{
    public AudioSource source;

    private void Start()
    {
    }
    public void Init(AudioClip clip, float volume)
    {
        source.clip = clip; // define the clip
        source.volume = volume;
        source.dopplerLevel = 0;
        source.spatialBlend = 1;
        source.time = 0;
        source.Play(); // start the sound

        StartCoroutine(WaitAndReturnToPool(clip.length));
    }

    IEnumerator WaitAndReturnToPool(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToPool();
    }
}
