using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAudioManager : MonoBehaviour {
    [SerializeField]
    private List<AudioClip> wallBounces;
    [SerializeField]
    private List<AudioClip> paddleBounces;

    private float _bounceVolume;

    private AudioSource _audioSource;
    void Start() {
        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _bounceVolume = PlayerPrefs.GetFloat("sfxVolume");
    }


    public void OnWallBounce() {
        int randIndex = Random.Range(0, wallBounces.Count);
        //Debug.LogError("Playing wall bounce number: " + randIndex);
        _audioSource.clip = wallBounces[randIndex];
        _audioSource.Play();
    }

    public void OnPaddleBounce() {
        int randIndex = Random.Range(0, paddleBounces.Count);
        //Debug.LogError("Playing paddle bounce number: " + randIndex);
        _audioSource.volume = _bounceVolume;
        _audioSource.clip = paddleBounces[randIndex];
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
    }

    public void SetBounceVolume(float volume) {
        _bounceVolume = volume;
        PlayerPrefs.SetFloat("sfxVolume", _bounceVolume);
        //play a sound for the user
        UIAudioManager uIAudioManager = FindObjectOfType<UIAudioManager>();
        uIAudioManager.PlayPlingSound(_bounceVolume);
    }
}
