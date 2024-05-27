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

    [SerializeField]
    private List<AudioClip> windSounds;

    [SerializeField]
    private List<AudioClip> splitBallSounds;

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
        if (Input.GetKeyUp(KeyCode.L)) {
            //_audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.Play();
        }
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

                case "Wind With Player":
                case "Wind Against Player":
                    audioClipList = windSounds;
                    break;

                case "Split Ball":
                    audioClipList = splitBallSounds;
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
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.Play();
    }
}
