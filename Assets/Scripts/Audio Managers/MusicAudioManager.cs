using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioManager : MonoBehaviour {

    [SerializeField]
    private List<AudioClip> relaxedMusic;

    [SerializeField]
    private List<AudioClip> intenseMusic;

    private float _musicVolume;

    private AudioSource _audioSource;

    void Start() {
        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _musicVolume = PlayerPrefs.GetFloat("musicVolume");
    }

    void Update() {
        //TODO: check if player is on last live and make music more intense
    }

    private void PlayRandomSong(List<AudioClip> songs) {
        if (songs.Count == 0)
            return;
        int randIndex = Random.Range(0, songs.Count);
        _audioSource.clip = songs[randIndex];
        _audioSource.volume = _musicVolume;
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
    }

    public void SetMusicVolume(float volume) {
        _musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", _musicVolume);

        //play a sound for the user
        UIAudioManager uIAudioManager = FindObjectOfType<UIAudioManager>();
        uIAudioManager.SetSoundToDo();
        uIAudioManager.PlaySoundAfterDelay(_musicVolume);
    }
}
