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

    [SerializeField]
    private AudioClip doSound;
    [SerializeField]
    private AudioClip reSound;
    [SerializeField]
    private AudioClip miSound;

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

    public void PlaySoundAfterDelay(float volume) {
        _audioSource.volume = volume;
        //_audioSource.clip = plingSound;
        StopAllCoroutines();
        StartCoroutine(PlaySoundAfterDelay());
    }

    public void SetSoundToDo() {
        _audioSource.clip = doSound;
    }
    public void SetSoundToRe() {
        _audioSource.clip = reSound;
    }
    public void SetSoundToMi() {
        _audioSource.clip = miSound;
    }

    private IEnumerator PlaySoundAfterDelay() {
        yield return new WaitForSeconds(0.15f);
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
