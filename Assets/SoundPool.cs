using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    public Sound soundPrefab;
    public Pool<Sound> soundPool;
    public int soundCount;

    void Start()
    {
        soundPool = new Pool<Sound>(soundPrefab, soundCount);
        foreach (Sound sound in soundPool.available) sound.gameObject.transform.SetParent(transform);
    }
}
