using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType
{
    FootStep,
    Gunshot,
    GunshotEnemy
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;
    public SoundPool soundPool;
    
    [Range(0, 1)]
    public float volume;

    //public Dictionary<SoundType, AudioClip> audioMap = new Dictionary<SoundType, AudioClip>();
    public Dictionary<SoundType, string> audioMap = new Dictionary<SoundType, string>();
    private void Awake()
    {
        current = this;

        //audioMap.Add(SoundType.FootStep, Resources.Load<AudioClip>("Sounds/FootStep"));
        //audioMap.Add(SoundType.Gunshot, Resources.Load<AudioClip>("Sounds/Gunshot"));

        audioMap.Add(SoundType.GunshotEnemy, "event:/enemy/enemy fire");
        audioMap.Add(SoundType.Gunshot, "event:/player/weapons");

    }

    void PlayClipAt(string path, Vector3 pos, float volume)
    {
        //Sound sound;
        //if (soundPool.soundPool.TryInstantiate(out sound, pos, Quaternion.identity))
        //{
        //    sound.Init(clip, volume);
        //}

        FMODUnity.RuntimeManager.PlayOneShot(path, pos);
    }


    public static void PlaySound(SoundType sound, Vector3 position, float multiplier)
    {
        current.PlaySoundInternal(sound, position, multiplier);
    }


    void PlaySoundInternal(SoundType sound, Vector3 position, float multiplier)
    {
        PlayClipAt(audioMap[sound], position, volume * multiplier);
    }
}
