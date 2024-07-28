using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;

    [Title("#BGM")]
    [TabGroup("BGM")] public AudioClip bgmClip;
    [TabGroup("BGM")] public float bgmVolume;
    [TabGroup("BGM")] private AudioSource bgmPlayer;

    [Title("#SFX")]
    [TabGroup("SFX")] public AudioClip[] sfxClips;
    [TabGroup("SFX")] public float sfxVolume;
    [TabGroup("SFX")] public int sfxChannels = 5; // Default channel count
    [TabGroup("SFX")] private AudioSource[] sfxPlayers;
    [TabGroup("SFX")] private int channelIndex;

    protected override void Awake()
    {
        base.Awake();

        // 배경음
        GameObject bgmObj = new GameObject("BgmPlayer");
        bgmObj.transform.parent = transform;
        bgmPlayer = bgmObj.AddComponent<AudioSource>();
        bgmPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Bgm")[0];
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        // 효과음
        sfxPlayers = new AudioSource[sfxChannels];
        for (int i = 0; i < sfxChannels; i++)
        {
            GameObject sfxObj = new GameObject($"SfxPlayer_{i}");
            sfxObj.transform.parent = transform;
            sfxPlayers[i] = sfxObj.AddComponent<AudioSource>();
            sfxPlayers[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Sfx")[0];
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    private void Start()
    {
        PlayBgm();
    }

    public void PlayBgm()
    {
        bgmPlayer.Play();
    }

    public void PlaySfx(int num)
    {
        if (num < 0 || num >= sfxClips.Length)
        {
            Debug.LogWarning("Invalid SFX clip index");
            return;
        }

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[num];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
