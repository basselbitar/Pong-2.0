using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> wallBounces;
    [SerializeField]
    private List<AudioClip> paddleBounces;

    private AudioSource _audioSource;
    void Start()
    {
        if(!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }


    public void OnWallBounce() {
        int randIndex = Random.Range(0, wallBounces.Count);
        Debug.LogError("Playing wall bounce number: " + randIndex);
        _audioSource.clip = wallBounces[randIndex];
        _audioSource.Play();
    }

    public void OnPaddleBounce() {
        int randIndex = Random.Range(0, paddleBounces.Count);
        Debug.LogError("Playing paddle bounce number: " + randIndex);
        _audioSource.clip = paddleBounces[randIndex];
        _audioSource.Play();
    }
}
