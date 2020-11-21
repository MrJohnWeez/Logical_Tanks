using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _soundtracks = null;
    private AudioSource _audioSource = null;

    private void Awake()
    {
        _audioSource = new GameObject("MainAudio").AddComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.maxDistance = 1000;
        _audioSource.minDistance = 0.01f;
        _audioSource.playOnAwake = false;
        SaveData.OnAudioChanged += ChangeAudio;
    }

    private void OnDestroy()
    {
        SaveData.OnAudioChanged -= ChangeAudio;
    }

    private void Update ()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = _soundtracks[Random.Range(0, _soundtracks.Length)];
            _audioSource.Play();
        }
    }

    private void Start() { ChangeAudio(); }

    private void ChangeAudio()
    {
        _audioSource.volume = SaveData.MusicLevel / (float)100;
    }
}
