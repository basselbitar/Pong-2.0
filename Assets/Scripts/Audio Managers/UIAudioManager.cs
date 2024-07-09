using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour {

    [SerializeField]
    private AudioClip clickSound;

    [SerializeField]
    private AudioClip confirmSound;

    [SerializeField]
    private AudioClip plingSound;

    [SerializeField]
    private AudioClip youWinSound;

    [SerializeField]
    private AudioClip youLoseSound;

    private AudioSource _audioSource;
    private float _sfxVolume;

    void Awake() {
        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        _sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SetSfxVolume(float sfxVolume) {
        _sfxVolume = sfxVolume;
    }

    public void PlayClickSound() {
        _audioSource.clip = clickSound;
        //_audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.volume = _sfxVolume;
        _audioSource.Play();
    }

    public void PlayConfirmSound() {
        _audioSource.clip = confirmSound;
        //_audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.volume = _sfxVolume;
        _audioSource.Play();
    }

    public void PlayPlingSound(float volume) {
        _audioSource.volume = volume;
        //StopCoroutine(PlayPlingSoundAfterDelay());
        StopAllCoroutines();
        StartCoroutine(PlayPlingSoundAfterDelay());
    }

    private IEnumerator PlayPlingSoundAfterDelay() {
        Debug.Log("Hello World");
        yield return new WaitForSeconds(0.15f);
        Debug.Log("Goodbye World");
        _audioSource.clip = plingSound;
        _audioSource.Play();
    }

    public void PlayYouWinSound() {
        _audioSource.clip = youWinSound;
        _audioSource.volume = _sfxVolume;
        _audioSource.Play();
    }

    public void PlayYouLoseSound() {
        _audioSource.clip = youLoseSound;
        _audioSource.volume = _sfxVolume;
        _audioSource.Play();
    }
}
