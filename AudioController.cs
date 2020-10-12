using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//MARK: - AudioController
public class AudioController : MonoBehaviour
{
    //MARK: - Properties 

    private static AudioController _instance;

    public static AudioController Instance
    {
        get
        {
            if (_instance == null)
            {
                //_instance = new GameObject("Audio Controller", typeof(AudioController)).GetComponent<AudioController>();

                Debug.LogError("Audio Player is NULL!");
            }

            return _instance;
        }
    }

    //TODO: Возможно стоит сделать разделение на массив музыки и массив SFX
    public Sound[] sounds;

    private int currentSceneIndex;

    //MARK: - Unity Life Cycle

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(this);

        CheckDefaultVolume();

        InitializationAllSound();
    }

    private void OnEnable()
    {
        PlayerPrefsController.OnMusicVolumeChange += SetMusicVolume;

        PlayerPrefsController.OnSFXVolumeChange += SetSFXVolume;
    }

    private void Start()
    {
        SwitchThemeMusic();
    }

    private void Update()
    {
        if (currentSceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SwitchThemeMusic();
        }
    }

    private void OnDisable()
    {
        PlayerPrefsController.OnMusicVolumeChange -= SetMusicVolume;

        PlayerPrefsController.OnSFXVolumeChange -= SetSFXVolume;
    }

    //MARK: - Private Methods

    private void CheckDefaultVolume()
    {
        if (!PlayerPrefsController.IsSetDefaultVolumeSettings)
        {
            PlayerPrefsController.SetDefaultVolumeInitialized();

            PlayerPrefsController.SetDefaultVolume();
        }
    }

    private void InitializationAllSound()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

            s.source.playOnAwake = s.playOnAwake;

            if (s.typeOfSound == Sound.Type.Music)
            {
                s.source.volume = PlayerPrefsController.GetMasterVolume();
            }
            else if (s.typeOfSound == Sound.Type.SFX)
            {
                s.source.volume = PlayerPrefsController.GetSFXVolume();
            }
        }
    }

    private void SwitchThemeMusic()
    {
        var activeScene = SceneManager.GetActiveScene();

        currentSceneIndex = activeScene.buildIndex;

        if (activeScene.buildIndex == 0)
        {
            Play("Splash");
        }
        else if (activeScene.name == "Level Selection" || activeScene.name == "Main Menu" || activeScene.name == "Options Screen" || activeScene.name == "Tutorial"
            || activeScene.name == "Localization Scene" || activeScene.name == "Nickname Scene")
        {
            if (ComparePlayingMusic() != "Menu")
            {
                StopMusic();

                Play("Menu");
            }
        }
        else if (activeScene.name == "Congratulations Scene")
        {
            StopMusic();

            Play("Congratulation", 0.75f);
        }
        else
        {
            if (ComparePlayingMusic() != "Theme")
            {
                StopMusic();

                Play("Theme");
            }
        }
    }

    private string ComparePlayingMusic()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.typeOfSound == Sound.Type.Music)
            {
                bool isPlaying = sound.source.isPlaying;

                if (isPlaying)
                {
                    return sound.soundName; 
                }
            }
        }
                return null;
    }

    private void StopMusic()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.typeOfSound == Sound.Type.Music)
            {
                sound.source.Stop();
        }
    }
    }

    private IEnumerator PlayRoutine(Sound sound ,float time)
    {
        yield return new WaitForSecondsRealtime(time);

        sound.source.Play();
    }

    //MARK: - Public Methods

    public void Play(string name, float time = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.soundName == name);

        if (s == null)
        {
            Debug.LogError($"Sound {name} not found!");

            return;
        }

        if (time == 0)
        {
            s.source.Play();
        }
        else if (time > 0)
        {
            StartCoroutine(PlayRoutine(s, time));
        }
    }

    public void PlayLoseSounds()
    {
        StartCoroutine(PlayLoseRoutine());
    }

    public void PlayWinSounds()
    {
        StartCoroutine(PlayWinRoutine());
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.typeOfSound == Sound.Type.Music)
            {
                s.source.volume = volume; 
            }
        }
    }

    public void SetSFXVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.typeOfSound == Sound.Type.SFX)
            {
                s.source.volume = volume;
            }
        }
    }

    //MARK: - Private Methods

    private IEnumerator PlayLoseRoutine()
    {
        string[] soundsTitle = { "Lose", "You Lose", "GG", "Easy", "Lose Laugh" };

        float[] timesToPlay = { 0.8f, 1f, 0.7f, 0.6f, 0f };

        for (int i = 0; i < soundsTitle.Length; i++)
        {
            Sound soundToPlay = Array.Find(sounds, sound => sound.soundName == soundsTitle[i]);

            if (soundToPlay == null)
            {
                Debug.LogError($"Sound {soundsTitle[i]} not found!");

                yield break;
            }

            soundToPlay.source.Play();

            yield return new WaitForSecondsRealtime(timesToPlay[i]);
        }
    }

    private IEnumerator PlayWinRoutine()
    {
        string[] soundsTitle = { "Win", "Win Laugh", "Well Played", "You Win" };

        float[] timesToPlay = { 0.45f, 0.9f, 1f, 0f };

        for (int i = 0; i < soundsTitle.Length; i++)
        {
            Sound soundToPlay = Array.Find(sounds, sound => sound.soundName == soundsTitle[i]);

            if (soundToPlay == null)
            {
                Debug.LogError($"Sound {soundsTitle[i]} not found!");

                yield break;
            }

            soundToPlay.source.Play();

            yield return new WaitForSecondsRealtime(timesToPlay[i]);
        }
    }
}
