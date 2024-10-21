using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    instance = obj.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    private Queue<AudioSource> audioSourcePool = new Queue<AudioSource>();
    private Dictionary<AudioClip, AudioSource> activeSources = new Dictionary<AudioClip, AudioSource>();

    [Header("Scenes to stop sounds")]
    public List<string> scenesToStopSounds = new List<string> { "1.menu_ui" };

    [Header("Pool Settings")]
    public int poolSize = 10;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitializeAudioSourcePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioSourcePool.Enqueue(source);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scenesToStopSounds.Contains(scene.name))
        {
            StopAllSounds();
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        if (clip == null) return;

        AudioSource source;
        if (activeSources.TryGetValue(clip, out source))
        {
            if (source.isPlaying)
            {
                // Optionally stop the old source if it's already playing
                // source.Stop();
            }
        }
        else
        {
            if (audioSourcePool.Count > 0)
            {
                source = audioSourcePool.Dequeue();
            }
            else
            {
                source = gameObject.AddComponent<AudioSource>();
            }

            activeSources[clip] = source;
        }

        // Configure and play the AudioSource
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    public void StopSound(AudioClip clip)
    {
        if (clip == null) return;

        if (activeSources.TryGetValue(clip, out AudioSource source))
        {
            source.Stop();
        }
    }

    public bool IsPlaying(AudioClip clip)
    {
        if (clip == null) return false;

        if (activeSources.TryGetValue(clip, out AudioSource source))
        {
            return source.isPlaying;
        }
        return false;
    }

    public void UnloadAudioClip(AudioClip clip)
    {
        if (clip == null) return;

        if (activeSources.TryGetValue(clip, out AudioSource source))
        {
            source.Stop();
            audioSourcePool.Enqueue(source);
            activeSources.Remove(clip);
        }
    }

    private void StopAllSounds()
    {
        foreach (var source in activeSources.Values)
        {
            source.Stop();
            audioSourcePool.Enqueue(source);
        }
        activeSources.Clear();
    }
}
