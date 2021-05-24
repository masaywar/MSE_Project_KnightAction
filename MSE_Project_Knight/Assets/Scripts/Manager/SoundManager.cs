using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> _audioDict;

    public Dictionary<string, AudioClip> audioDict 
    {
        get 
        {
            if (_audioDict == null)
                _audioDict = new Dictionary<string, AudioClip>();

            return _audioDict;
        }
    }

    [SerializeField]
    private List<AudioSource> _audioSources = new List<AudioSource>();
    public List<AudioSource> audioSources
    {
        get => _audioSources;
    }

    public void Initialize()
    {
        var effectClips = Resources.LoadAll<AudioClip>("Sound/Effect");
        effectClips.ForEach(clip =>audioDict.Add(clip.name, clip));

        var musicClips = Resources.LoadAll<AudioClip>("Sound/Music");
        musicClips.ForEach(clip => audioDict.Add(clip.name, clip));

        GameObject effectPlayer = new GameObject("EffectPlayer");
        GameObject musicPlayer = new GameObject("MusicPlayer");

        effectPlayer.AddComponent<AudioSource>();
        musicPlayer.AddComponent<AudioSource>();

        effectPlayer.transform.SetParent(transform);
        musicPlayer.transform.SetParent(transform);

        audioSources.Add(effectPlayer.GetComponent<AudioSource>());
        audioSources.Add(musicPlayer.GetComponent<AudioSource>());

        DontDestroyOnLoad(this);
    }

    public void StopAll()
    {
        audioSources.ForEach(source => source.Stop());
    }
    public void PlayOneShot(string name)
    {
        AudioSource source = GetEffectSource();

        if (audioDict.TryGetValue(name, out var clip))
        {
            source.Stop();
            source.PlayOneShot(clip);
        }
    }

    public void PlayMusic(string name, bool loop)
    {
        AudioSource source = GetMusicSource();

        if (audioDict.TryGetValue(name, out var clip))
        {
            source.Stop();
            source.clip = clip;
            source.Play();
            source.loop = loop;
        }
    }

    public AudioSource GetEffectSource()
    { 
        return audioSources.Find(source => source.name == "EffectPlayer");
    }

    public AudioSource GetMusicSource()
    {
        return audioSources.Find(source => source.name == "MusicPlayer");
    }
}
