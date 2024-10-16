using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAudioManager : MonoBehaviour {

    [SerializeField]
    private List<AudioClip> relaxedMusic;

    [SerializeField]
    private List<AudioClip> intenseMusic;

    [SerializeField]
    private List<AudioClip> superIntenseMusic;

    private float _musicVolume;

    private AudioSource _audioSource;
    private int _intensityLevel; // 0 for normal gameplay, 1 for intense game, 2 for super intense game

    void Start() {
        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _musicVolume = PlayerPrefs.GetFloat("musicVolume");
        //Debug.Log("Initially, Music volume was: " + _musicVolume);

    }

    public void UpdateSoundtrack(bool intenseGame, bool superIntenseGame) {
        if ((_intensityLevel == 0 && (intenseGame || superIntenseGame)) || (_intensityLevel == 1 && superIntenseGame)) {
            if (superIntenseGame) {
                _intensityLevel = 2;
                PlaySuperIntenseMusic();
            }
            else {
                _intensityLevel = 1;
                PlayIntenseMusic();
            }
        }
        if (_intensityLevel > 0 && !intenseGame && !superIntenseGame) {
            _intensityLevel = 0;
            PlayRelaxedMusic();
        }
    }

    public void PlayRelaxedMusic() {
        PlayRandomSong(relaxedMusic);
    }

    public void PlayIntenseMusic() {
        PlayRandomSong(intenseMusic);
    }

    public void PlaySuperIntenseMusic() {
        PlayRandomSong(superIntenseMusic);
    }

    private void PlayRandomSong(List<AudioClip> songs) {
        if (songs.Count == 0)
            return;
        int randIndex = Random.Range(0, songs.Count);
        _audioSource.clip = songs[randIndex];
        _audioSource.volume = _musicVolume;
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
        //Debug.Log("Music volume is: " + _musicVolume);
    }

    public void SetMusicVolume(float volume) {
        _musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", _musicVolume);

        //play a sound for the user
        UIAudioManager uIAudioManager = FindObjectOfType<UIAudioManager>();
        uIAudioManager.SetSoundToDo();
        uIAudioManager.PlaySoundAfterDelay(_musicVolume);
    }

    public void Stop() {

        _audioSource.Stop();
    }
}
