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

    public AudioSource _relaxedAudioSource;
    public AudioSource _intenseAudioSource;
    public AudioSource _superIntenseAudioSource;
    private int _intensityLevel; // 0 for normal gameplay, 1 for intense game, 2 for super intense game

    public void Start() {
        _musicVolume = PlayerPrefs.GetFloat("musicVolume");
        //Debug.Log("Initially, Music volume was: " + _musicVolume);
    }

    private void Update() {
        //Debug.Log("Intensity Level is: " + _intensityLevel);
    }

    public void UpdateSoundtrack(bool intenseGame, bool superIntenseGame) {

        if(superIntenseGame) {
            if(_intensityLevel != 2) {
                _intensityLevel = 2;
                PlaySuperIntenseMusic();
            }
        } else if(intenseGame) {
            _intensityLevel = 1;
            PlayIntenseMusic();
        } else {
            _intensityLevel = 0;
            _intenseAudioSource.volume = 0;
            _superIntenseAudioSource.volume = 0;
        }
    }

    public void PlayRelaxedMusic() {
        PlayRandomSong(_relaxedAudioSource, relaxedMusic, _musicVolume);
        //PlayRandomSong(_intenseAudioSource, intenseMusic, 0);
        //PlayRandomSong(_superIntenseAudioSource, superIntenseMusic, 0);
    }

    public void PlayIntenseMusic() {
        //PlayRandomSong(intenseMusic);
        PlayRandomSong(_intenseAudioSource, intenseMusic, _musicVolume);
        _relaxedAudioSource.Stop();
        _superIntenseAudioSource.Stop();
    }

    public void PlaySuperIntenseMusic() {
        //PlayRandomSong(superIntenseMusic);
        PlayRandomSong(_superIntenseAudioSource, superIntenseMusic, _musicVolume);
        _superIntenseAudioSource.volume = _musicVolume;
        _relaxedAudioSource.Stop();
        _intenseAudioSource.Stop();
    }

    private void PlayRandomSong(AudioSource audioSource,  List<AudioClip> songs, float volume) {
        if (songs.Count == 0)
            return;
        int randIndex = Random.Range(0, songs.Count);
        audioSource.clip = songs[randIndex];
        audioSource.volume = volume;
        //_audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
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
        _relaxedAudioSource.Stop();
        _intenseAudioSource.Stop();
        _superIntenseAudioSource.Stop();
    }
}
