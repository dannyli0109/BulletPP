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

    public Dictionary<SoundType, AudioClip> audioMap = new Dictionary<SoundType, AudioClip>();
    private void Awake()
    {
        current = this;

        audioMap.Add(SoundType.FootStep, Resources.Load<AudioClip>("Sounds/FootStep"));
        audioMap.Add(SoundType.Gunshot, Resources.Load<AudioClip>("Sounds/Gunshot"));
        audioMap.Add(SoundType.GunshotEnemy, Resources.Load<AudioClip>("Sounds/GunshotEnemy"));
    }

    void PlayClipAt(AudioClip clip, Vector3 pos, float volume)
    {
        Sound sound;
        if (soundPool.soundPool.TryInstantiate(out sound, pos, Quaternion.identity))
        {
            sound.Init(clip, volume);
        }
        //GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        //tempGO.transform.position = pos; // set its position
        //AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        //aSource.clip = clip; // define the clip
        //// set other aSource properties here, if desired
        //aSource.volume = volume;
        //aSource.dopplerLevel = 0;
        //aSource.spatialBlend = 1;
        //aSource.Play(); // start the sound
        //Destroy(tempGO, clip.length); // destroy object after clip duration
        //return aSource; // return the AudioSource reference
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
