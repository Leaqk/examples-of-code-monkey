using System;
using UnityEngine;

//MARK: - PlayerPrefsController

public class PlayerPrefsController : MonoBehaviour
{
    //MARK: - Properties

    public static bool IsEndlessLevelOpen
    {
        get
        {
            return PlayerPrefs.GetInt(ENDLESS_LEVEL_OPEN) == 0 ? false : true;
        }
    }

    public static bool IsSetDefaultVolumeSettings
    {
        get
        {
            return PlayerPrefs.GetInt(DEFAULT_VOLUME_INITIALIZATION) == 0 ? false : true;
        }
    }

    public static bool IsShowedTutorial
    {
        get
        {
            return PlayerPrefs.GetInt(TUTORIAL_IS_SHOWED) == 0 ? false : true;
        }
    }

    public static bool IsChoosenLanguage
    {
        get
        {
            return PlayerPrefs.GetInt(LANGUAGE_IS_CHOOSEN) == 0 ? false : true;
        }
    }

    public static bool IsEnteredNickname
    {
        get
        {
            return PlayerPrefs.GetInt(NICKNAME_IS_ENTERED) == 0 ? false : true;
        }
    }

    public static string userIdentifier
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    public static Action<float> OnMusicVolumeChange;

    public static Action<float> OnSFXVolumeChange;

    const string LAST_NONZERO_MUSIC_VOLUME_KEY = "last nonzero music volume";

    const string LAST_NONZERO_SFX_VOLUME_KEY = "last nonzero sfx volume";

    const string MASTER_VOLUME_KEY = "master volume";

    const string SFX_VOLUME_KEY = "sfx volume";

    const string DIFFICULTY_KEY = "difficulty";

    const string CURRENT_LEVEL_KEY = "complete level";

    const string TUTORIAL_IS_SHOWED = "isShowedTutorial";

    const string LANGUAGE_IS_CHOOSEN = "isChoosenLanguage";

    const string NICKNAME_IS_ENTERED = "isEnteredNickname";

    const string ENDLESS_LEVEL_OPEN = "isEndlessLevelOpen";

    const string DEFAULT_VOLUME_INITIALIZATION = "isInitializedDefaultVolume";

    public const string NICKNAME = "CurrentNickname";

    const string NO_NICKNAME = "Noname";

    const string CURRENT_SCORE = "currentUserScore";

    const string CURRENT_TIME = "currentEndlessLevelTime";

    const float MIN_MUSIC_VOLUME = 0f;

    const float MAX_MUSIC_VOLUME = 0.5f;

    const float MIN_SFX_VOLUME = 0f;

    const float MAX_SFX_VOLUME = 0.7f;

    const float MIN_DIFFICULTY = 0f;

    public const float MAX_DIFFICULTY = 1f;

    const int MIN_SCENE_INDEX = 5;

    const int MAX_SCENE_INDEX = 14;

    public const float DEFAULT_MUSIC_VOLUME = 0.25f;

    public const float DEFAULT_SFX_VOLUME = 0.65f;

    //MARK: - Public Methods

    public static void SetEndlessLevelOpen()
    {
        PlayerPrefs.SetInt(ENDLESS_LEVEL_OPEN, 1);
    }

    public static void SetDefaultVolume()
    {
        SetMusicVolume(DEFAULT_MUSIC_VOLUME);

        SetSFXVolume(DEFAULT_SFX_VOLUME);
    }

    public static void SetCurrentScore(int scores)
    {
        PlayerPrefs.SetInt(CURRENT_SCORE, scores);
    }

    public static int GetCurrentUserScore()
    {
        return PlayerPrefs.GetInt(CURRENT_SCORE);
    }

    public static void SetCurrentEndlessLevelTime(string time)
    {
        PlayerPrefs.SetString(CURRENT_TIME, time);
    }

    public static string GetCurrentEndlessLevelTime()
    {
        return PlayerPrefs.GetString(CURRENT_TIME); ;
    }

    public static void SetNickname(string userNickname)
    {
        var nickname = userNickname.Length > 0 ? userNickname : NO_NICKNAME;

        PlayerPrefs.SetString(NICKNAME, nickname);
    }

    public static void SetNicknameEntered()
    {
        PlayerPrefs.SetInt(NICKNAME_IS_ENTERED, 1);
    }

    public static string GetNickname()
    {
        return PlayerPrefs.GetString(NICKNAME);
    }

    public static void SetTutorialShowed()
    {
        PlayerPrefs.SetInt(TUTORIAL_IS_SHOWED, 1);
    }
    public static void SetDefaultVolumeInitialized()
    {
        PlayerPrefs.SetInt(DEFAULT_VOLUME_INITIALIZATION, 1);
    }

    public static void SetLanguageChoosen()
    {
        PlayerPrefs.SetInt(LANGUAGE_IS_CHOOSEN, 1);
    }

    public static void SetCurrentLevel(int sceneIndex)
    {
        if (sceneIndex >= MIN_SCENE_INDEX && sceneIndex <= MAX_SCENE_INDEX)
        {
            Debug.Log("Current level set to " + sceneIndex);

            PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, sceneIndex);
        }
        else
        {
            Debug.LogError("Wrong scene index!");
        }
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(CURRENT_LEVEL_KEY);
    }

    public static int GetMaxLevel()
    {
        return MAX_SCENE_INDEX;
    }

    public static int GetMinLevel()
    {
        return MIN_SCENE_INDEX;
    }

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_MUSIC_VOLUME && volume <= MAX_MUSIC_VOLUME)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);

            if (volume != 0)
            {
                PlayerPrefs.SetFloat(LAST_NONZERO_MUSIC_VOLUME_KEY, volume);
            }

            if (OnMusicVolumeChange != null)
            {
                OnMusicVolumeChange(volume);
            }
        }
        else
        {
            Debug.LogError("Sound volume out of range");
        }
    }

    public static void SetSFXVolume(float volume)
    {
        if (volume >= MIN_SFX_VOLUME && volume <= MAX_SFX_VOLUME)
        {
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);

            if (volume != 0)
            {
                PlayerPrefs.SetFloat(LAST_NONZERO_SFX_VOLUME_KEY, volume);
            }

            if (OnSFXVolumeChange != null)
            {
                OnSFXVolumeChange(volume);
            }
        }
        else
        {
            Debug.LogError("SFX volume out of range");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_KEY);
    }

    public static float GetLastNonZeroMusicVolume()
    {
        return PlayerPrefs.GetFloat(LAST_NONZERO_MUSIC_VOLUME_KEY);
    }

    public static float GetLastNonZeroSFXVolume()
    {
        return PlayerPrefs.GetFloat(LAST_NONZERO_SFX_VOLUME_KEY);
    }

    public static void SetDifficulty(float difficulty)
    {
        if (difficulty >= MIN_DIFFICULTY && difficulty <= MAX_DIFFICULTY)
        {
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
        }
        else
        {
            Debug.LogError("Difficulty setting is not in range");
        }
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }
}
