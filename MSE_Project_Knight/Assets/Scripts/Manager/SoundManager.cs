using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> _audioDict;
    private Dictionary<string, AudioSource> _sourceDict;

    public Dictionary<string, AudioClip> audioDict 
    {
        get 
        {
            if (_audioDict == null)
                _audioDict = new Dictionary<string, AudioClip>();

            return _audioDict;
        }
    }

    public Dictionary<string, AudioSource> sourceDict
    {
        get
        {
            if (_sourceDict == null)
                _sourceDict = new Dictionary<string, AudioSource>();

            return _sourceDict;
        }
    }

    private void Awake()
    {
        var clips = Resources.LoadAll<AudioClip>("Sound/Effect");

        clips.ForEach(clip =>
        {
            string name = clip.name;
            audioDict.Add(name, clip);
        });

    }

    public void PlayOneShot(string name, AudioSource audio)
    {
        if (audioDict.TryGetValue(name, out var clip))
        {
            audio.Stop();
            audio.PlayOneShot(clip);
        }
    }
}
