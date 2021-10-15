using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FMODUnity.RuntimeManager;
using FMODUnity;
public enum SoundType
{
    FootStep,
    Gunshot,
    GunshotEnemy,
    BlasterHit,
    RocketHit,
    LaserHit
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;
    public SoundPool soundPool;
    
    [Range(0, 1)]
    public float volume;

    //public Dictionary<SoundType, AudioClip> audioMap = new Dictionary<SoundType, AudioClip>();
    public Dictionary<SoundType, string> audioMap = new Dictionary<SoundType, string>();

    FMOD.Studio.EventInstance atmos;
    FMOD.Studio.EventInstance levelMusic;
    List<FMOD.Studio.EventInstance> soundPlaying = new List<FMOD.Studio.EventInstance>();

    private void Awake()
    {
        current = this;

        //audioMap.Add(SoundType.FootStep, Resources.Load<AudioClip>("Sounds/FootStep"));
        //audioMap.Add(SoundType.Gunshot, Resources.Load<AudioClip>("Sounds/Gunshot"));

        audioMap.Add(SoundType.GunshotEnemy, "event:/enemy/enemy fire");
        audioMap.Add(SoundType.Gunshot, "event:/player/weapons");
        audioMap.Add(SoundType.BlasterHit, "event:/enemy/blaster hit");
        audioMap.Add(SoundType.RocketHit, "event:/enemy/rocket hit");
        audioMap.Add(SoundType.LaserHit, "event:/enemy/laser hit");

        atmos = CreateInstance(PathToGUID("event:/envioroment/atmos"));
        levelMusic = CreateInstance(PathToGUID("event:/music/level music"));

        atmos.setVolume(volume);
        atmos.start();

        levelMusic.setVolume(volume);
        levelMusic.start();
    }

    private void Update()
    {
        if (GameManager.current.GetState() != GameState.Shop)
        {
            levelMusic.setVolume(volume);
            atmos.setVolume(volume * 0.5f);
            if (!IsPlaying(levelMusic))
            {
                levelMusic.start();
            }
        }
        else
        {
            levelMusic.setVolume(volume * 0.2f);
            atmos.setVolume(volume * 0.1f);

        }

        //for (int i = 0; i < soundPlaying.Count; i++)
        //{
        //    if (!IsPlaying(soundPlaying[i]))
        //    {
        //        soundPlaying[i].release();
        //        soundPlaying.RemoveAt(i);
        //        i--;
        //    }
        //}
    }



    List<FMOD.Studio.EventInstance> CreateAudioPool(string path, int amounts)
    {
        List<FMOD.Studio.EventInstance> pool = new List<FMOD.Studio.EventInstance>();
        for (int i = 0; i < amounts; i++)
        {
            var instance = CreateInstance(PathToGUID(path));
            pool.Add(instance);
        }
        return pool;
    }


    FMOD.Studio.EventInstance PlayClipAt(string path, Vector3 pos, float volume)
    {
        var instance = CreateInstance(PathToGUID(path));
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
        instance.setVolume(volume);
        instance.start();
        instance.release();
        return instance;
    }


    public static FMOD.Studio.EventInstance PlaySound(SoundType sound, Vector3 position, float multiplier)
    {
        return current.PlaySoundInternal(sound, position, multiplier);
    }

    public bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }


    FMOD.Studio.EventInstance PlaySoundInternal(SoundType sound, Vector3 position, float multiplier)
    {
        return PlayClipAt(audioMap[sound], position, volume * multiplier);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < soundPlaying.Count; i++)
        {
            soundPlaying[i].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            soundPlaying[i].release();
        }

        atmos.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        atmos.release();
        levelMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        levelMusic.release();
    }
}
