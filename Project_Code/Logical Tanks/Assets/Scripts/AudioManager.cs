using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource = null;
    private AudioClip[] _soundtracks = null;

    private void Awake()
    {
        _audioSource = new GameObject("MainAudio").AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.maxDistance = 1000;
        _audioSource.minDistance = 0.01f;
        _audioSource.playOnAwake = false;
        SaveData.OnAudioChanged += () => _audioSource.volume = SaveData.MusicLevel / 100;
    }

    private void Update ()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _soundtracks[Random.Range(0, _soundtracks.Length)];
            _audioSource.Play();
        }
    }

    private void Start()
    {
        _audioSource.volume = SaveData.MusicLevel / 100;
    }
}
