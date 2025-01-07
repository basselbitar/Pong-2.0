using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> scoreSounds;

    [SerializeField]
    private List<AudioClip> scoredAtSounds;

    private float _scoreVolume;

    private AudioSource _audioSource;

    void Start()
    {
        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _scoreVolume = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void PlayScoreSound() {
        PlayRandomClip(scoreSounds);
    }

    public void PlayScoredAtSound() {
        PlayRandomClip(scoredAtSounds);
    }

    private void PlayRandomClip(List<AudioClip> sounds) {
        _scoreVolume = PlayerPrefs.GetFloat("sfxVolume");
        if (sounds.Count == 0)
            return;
        int randIndex = Random.Range(0, sounds.Count);
        _audioSource.clip = sounds[randIndex];
        _audioSource.volume = _scoreVolume;
        //Debug.Log("Score volume = " + _scoreVolume);
        //Debug.LogError("Playing track : " + _audioSource.clip.name);
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
    }
}
