using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound Data", order = 0)]
public class Sound : ScriptableObject
{
    public enum Type
    {
        Music,
        SFX
    }

    public Type typeOfSound;

    public string soundName;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;
}