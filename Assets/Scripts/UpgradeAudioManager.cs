using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> expansionSounds;
    [SerializeField]
    private List<AudioClip> shrinkingSounds;

    [SerializeField]
    private List<AudioClip> fasterSounds;
    [SerializeField]
    private List<AudioClip> slowerSounds;

    [SerializeField]
    private List<AudioClip> bonusLifeSounds;

    [SerializeField]
    private List<AudioClip> flipControlsSounds;

    private float timeOfLastSound;
    private float threshold;

    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        threshold = 2f;
        timeOfLastSound = Time.time - threshold; //allows sounds to play from the beginning

        if (!TryGetComponent<AudioSource>(out _audioSource)) {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollectUpgrade(Upgrade upgrade) {
        if (Time.time > timeOfLastSound + threshold) {
            List<AudioClip> audioClipList;

            string upgradeName = upgrade.GetData().GetName();
            switch(upgradeName) {
                case "Longer Paddle":
                case "Longer Both Paddles":
                    audioClipList = expansionSounds;
                    break;

                case "Shorter Paddle":
                case "Shorter Enemy Paddle":
                case "Shorter Both Paddles":
                    audioClipList = shrinkingSounds;
                    break;

                case "Faster Paddle":
                case "Faster Both Paddles":
                    audioClipList = fasterSounds;
                    break;

                case "Slower Paddle":
                case "Slower Enemy Paddle":
                case "Slower Both Paddles":
                    audioClipList = slowerSounds;
                    break;

                case "Bonus Life":
                    audioClipList = bonusLifeSounds;
                    break;

                case "Flip Enemy Controls":
                case "Flip Controls":
                case "Flip Both Controls":
                    audioClipList = flipControlsSounds;
                    break;

                default:
                    Debug.LogError("Something went wrong, can't find correct audio list");
                    audioClipList = new();
                    break;
            }

            PlayRandomClip(audioClipList);
            timeOfLastSound = Time.time;
        }
    }

    private void PlayRandomClip(List<AudioClip> sounds) {
        if (sounds.Count == 0)
            return;
        int randIndex = Random.Range(0, sounds.Count);
        _audioSource.clip = sounds[randIndex];
        _audioSource.Play();
    }
}
