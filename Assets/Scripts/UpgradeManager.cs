using Alteruna;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UpgradeData;

public class UpgradeManager : MonoBehaviour {
    [SerializeField]
    private GameManager _gameManager;
    public List<UpgradeData> upgrades;
    private Spawner _spawner;

    private const float MIN_X = -5;
    private const float MIN_Y = -4;
    private const float MAX_X = 5;
    private const float MAX_Y = 4;

    private const float initialSpawnUpgradeThreshold = 2f;
    private float spawnUpgradeThreshold;
    private float timeSinceLastSpawnedUpgrade = 0f;

    private int totalWeight = 0;

    private Paddle p1Paddle;
    private Paddle p2Paddle;

    public Sprite[] upgradeIcons;
    public GameObject windEffect;
    private int windsActiveCount = 0;
    private int windDirection = 1; //becomes -1 if flipped direction

    [SerializeField]
    public int upgradeIndex; //debug purposes

    private UpgradeAudioManager _upgradeAudioManager;
    private Ball[] _balls;

    void Start() {
        upgradeIndex = 0;
        PopulateUpgrades();
        _spawner = FindObjectOfType<Spawner>();
        spawnUpgradeThreshold = initialSpawnUpgradeThreshold;
        _upgradeAudioManager = FindObjectOfType<UpgradeAudioManager>();
    }

    private void FixedUpdate() {
        //disable scripts on the non-host player
        if (!_gameManager.IsGamePlaying())
            return;
        if (windsActiveCount > 0) {
            _balls = FindObjectsOfType<Ball>();
            foreach (var _ball in _balls) {
                _ball.AddForce(new Vector2(windDirection * windsActiveCount * 5, 0));
                Debug.Log("Wind Blowing");
            }
        }
    }

    private void Initialize() {
        p1Paddle = _gameManager.GetPaddle1();
        p2Paddle = _gameManager.GetPaddle2();
    }

    private void PopulateUpgrades() {
        upgrades = new List<UpgradeData>();

        //buffs                                                                amount, duration, weight, iconIndex
        UpgradeData longerPaddle = new(101, "Longer Paddle", Type.Buff, Aoe.Self, 1.2f, 3f, 4, 1);                     //id = 0
        UpgradeData fasterPaddle = new(102, "Faster Paddle", Type.Buff, Aoe.Self, 1.5f, 8f, 4, 2);                     //id = 1
        UpgradeData bonusLife = new(103, "Bonus Life", Type.Buff, Aoe.Self, 1, 0f, 1, 3);                              //id = 2
        UpgradeData shorterEnemyPaddle = new(104, "Shorter Enemy Paddle", Type.Buff, Aoe.Other, 0.8f, 3f, 3, 11);      //id = 3
        UpgradeData slowerEnemyPaddle = new(105, "Slower Enemy Paddle", Type.Buff, Aoe.Other, 0.7f, 3f, 1, 12);        //id = 4
        UpgradeData flipEnemyControls = new(106, "Flip Enemy Controls", Type.Buff, Aoe.Other, 0, 3f, 2, 4);            //id = 5
        UpgradeData windBuff = new(107, "Wind With Player", Type.Buff, Aoe.Self, 0, 8f, 2, 5);                         //id = 6

        upgrades.Add(longerPaddle);
        upgrades.Add(fasterPaddle);
        upgrades.Add(bonusLife);
        upgrades.Add(shorterEnemyPaddle);
        upgrades.Add(slowerEnemyPaddle);
        upgrades.Add(flipEnemyControls);
        upgrades.Add(windBuff);

        //nerfs                                                                     //amount, duration, weight, iconIndex
        UpgradeData shorterPaddle = new(201, "Shorter Paddle", Type.Nerf, Aoe.Self, 0.8f, 3f, 2, 11);                   //id = 7
        UpgradeData slowerPaddle = new(202, "Slower Paddle", Type.Nerf, Aoe.Self, 0.7f, 3f, 2, 12);                     //id = 8
        UpgradeData flipControls = new(203, "Flip Controls", Type.Nerf, Aoe.Self, 0, 3f, 1, 4);                         //id = 9
        UpgradeData windNerf = new(204, "Wind Against Player", Type.Nerf, Aoe.Other, 0, 8f, 2, 5);                      //id = 10

        upgrades.Add(shorterPaddle);
        upgrades.Add(slowerPaddle);
        upgrades.Add(flipControls);
        upgrades.Add(windNerf);

        //neutrals
        UpgradeData shorterBothPaddles = new(301, "Shorter Both Paddles", Type.Neutral, Aoe.Both, 0.8f, 3f, 1, 11);     //id = 11
        UpgradeData longerBothPaddles = new(302, "Longer Both Paddles", Type.Neutral, Aoe.Both, 1.2f, 3f, 2, 1);        //id = 12
        UpgradeData fasterBothPaddles = new(303, "Faster Both Paddles", Type.Neutral, Aoe.Both, 1.4f, 3f, 1, 2);          //id = 13
        UpgradeData slowerBothPaddles = new(304, "Slower Both Paddles", Type.Neutral, Aoe.Both, 0.7f, 3f, 1, 12);       //id = 14
        UpgradeData flipBothControls = new(305, "Flip Both Controls", Type.Neutral, Aoe.Both, 0, 3f, 1, 4);             //id = 15

        upgrades.Add(shorterBothPaddles);
        upgrades.Add(longerBothPaddles);
        upgrades.Add(fasterBothPaddles);
        upgrades.Add(slowerBothPaddles);
        upgrades.Add(flipBothControls);

    }

    private void Update() {
        if (!_gameManager.IsGamePlaying())
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            upgradeIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            upgradeIndex = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            upgradeIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            upgradeIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            upgradeIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            upgradeIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            upgradeIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            upgradeIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            upgradeIndex = 8;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            upgradeIndex += 10;
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            SpawnUpgrade();
        }
        if (_gameManager.IsGamePlaying()) {
            CheckSpawnUpgrade();
        }
    }
    // ticks the timer and spawns random upgrades at a set interval
    public void CheckSpawnUpgrade() {
        timeSinceLastSpawnedUpgrade += Time.deltaTime;

        if (timeSinceLastSpawnedUpgrade >= spawnUpgradeThreshold) {
            //randomize and announce an index
            upgradeIndex = GenerateRandomUpgradeIndex();
            SpawnUpgrade();
            timeSinceLastSpawnedUpgrade = 0;
        }
    }

    public void SpawnUpgrade() {
        Vector3 pos = new(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0f);
        GameObject upgradeGO = _spawner.Spawn(1, pos, Quaternion.identity, new Vector3(1f, 1f, 1f));

        Upgrade upgrade = upgradeGO.GetComponent<Upgrade>();
        //UpgradeData upgradeData = upgrades[Random.Range(0, upgrades.Count)]; //TODO: make upgrades not equally probably from each other
        UpgradeData upgradeData = upgrades[upgradeIndex]; //TODO: make upgrades not equally probably from each other

        upgrade.BroadcastRemoteMethod("SetData", upgradeData);

        upgrade.BroadcastRemoteMethod("ColorUpgrade");
    }

    public int GenerateRandomUpgradeIndex() {
        // use the upgrades' weights to return a weighted random choice of an upgrade
        if (totalWeight == 0) {
            foreach (var upgrade in upgrades) {
                totalWeight += upgrade.GetWeight();
            }
            Debug.Log("Total weight is: " + totalWeight);
        }
        int randIndex = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        for (int i = 0; i < upgrades.Count; i++) {
            cumulativeWeight += upgrades[i].GetWeight();
            if (randIndex < cumulativeWeight) {
                //Debug.Log("Random upgrade chosen has index: " + i); //useful when debugging to check prob distribution is working nicely
                return i;
            }
        }

        return -1;
    }




    public void PickupUpgrade(Upgrade upgrade) {
        //disable on non-host player
        if (!_gameManager.IsGamePlaying())
            return;

        if (p1Paddle == null || p2Paddle == null) {
            Debug.LogError("STILL NULL");
            Initialize();

        }

        //play the appropriate sound
        _upgradeAudioManager.OnCollectUpgrade(upgrade);

        string upgradeName = upgrade.GetData().GetName();
        UpgradeData.Aoe aoe = upgrade.GetData().GetAoe();
        float amount = upgrade.GetData().GetAmount();
        float duration = upgrade.GetData().GetDuration();

        Paddle targetPaddle = GetTargetPaddle(aoe);

        switch (upgradeName) {
            //buffs
            case "Longer Paddle":
                StartCoroutine(ModifyLength(targetPaddle, amount, duration));
                break;
            case "Faster Paddle":
                StartCoroutine(ModifySpeed(targetPaddle, amount, duration));
                break;
            case "Bonus Life":
                StartCoroutine(IncrementLives(targetPaddle));
                break;
            case "Shorter Enemy Paddle":
                StartCoroutine(ModifyLength(targetPaddle, amount, duration));
                break;
            case "Slower Enemy Paddle":
                StartCoroutine(ModifySpeed(targetPaddle, amount, duration));
                break;
            case "Flip Enemy Controls":
                StartCoroutine(FlipControls(targetPaddle, duration));
                break;
            case "Wind With Player":
                StartCoroutine(ActivateWind(targetPaddle, duration));
                break;

            //nerfs
            case "Shorter Paddle":
                StartCoroutine(ModifyLength(targetPaddle, amount, duration));
                break;
            case "Slower Paddle":
                StartCoroutine(ModifySpeed(targetPaddle, amount, duration));
                break;
            case "Flip Controls":
                StartCoroutine(FlipControls(targetPaddle, duration));
                break;
            case "Wind Against Player":
                StartCoroutine(ActivateWind(targetPaddle, duration));
                break;

            //neutrals
            case "Shorter Both Paddles":
                StartCoroutine(ModifyLength(p1Paddle, amount, duration));
                StartCoroutine(ModifyLength(p2Paddle, amount, duration));
                break;
            case "Longer Both Paddles":
                StartCoroutine(ModifyLength(p1Paddle, amount, duration));
                StartCoroutine(ModifyLength(p2Paddle, amount, duration));
                break;
            case "Faster Both Paddles":
                StartCoroutine(ModifySpeed(p1Paddle, amount, duration));
                StartCoroutine(ModifySpeed(p2Paddle, amount, duration));
                break;
            case "Slower Both Paddles":
                StartCoroutine(ModifySpeed(p1Paddle, amount, duration));
                StartCoroutine(ModifySpeed(p2Paddle, amount, duration));
                break;

            case "Flip Both Controls":
                StartCoroutine(FlipControls(p1Paddle, duration));
                StartCoroutine(FlipControls(p2Paddle, duration));
                break;

            default:
                Debug.LogError("Unknown Upgrade has been picked up");
                break;
        }
    }
    public void DestroyUpgrade(Upgrade upgrade) {
        _spawner.Despawn(upgrade.gameObject);
    }

    private IEnumerator ModifyLength(Paddle p, float amount, float duration) {
        ModifyLength(p, amount);
        yield return new WaitForSeconds(duration);
        ModifyLength(p, 1f / amount);
    }
    private IEnumerator ModifySpeed(Paddle p, float amount, float duration) {
        ModifySpeed(p, amount);
        yield return new WaitForSeconds(duration);
        ModifySpeed(p, 1f / amount);
    }

    private IEnumerator IncrementLives(Paddle p) {
        if (p == p1Paddle) {
            _gameManager.Player1GainsLife();
        }
        else if (p == p2Paddle) {
            _gameManager.Player2GainsLife();
        }
        yield return null;
    }

    private IEnumerator FlipControls(Paddle p, float duration) {
        FlipControls(p);
        yield return new WaitForSeconds(duration);
        FlipControls(p);
    }

    private IEnumerator ActivateWind(Paddle p, float duration) {
        windsActiveCount++;
        SetWind(p, windsActiveCount);
        yield return new WaitForSeconds(duration);
        windsActiveCount--;
        DeactivateWind(windsActiveCount);
    }

    private Paddle GetTargetPaddle(Aoe aoe) {
        // If aoe is self, we keep the targetIndex as the paddle that touched the ball last
        // If aoe is other, we swap by doing (1 - index)
        int ballTouchedBy = _gameManager.GetBallTouchedBy();
        int targetIndex = (aoe == Aoe.Self) ? ballTouchedBy : (1 - ballTouchedBy);
        return targetIndex == 0 ? p1Paddle : p2Paddle;
    }

    public void ModifyLength(Paddle p, float amount) {

        p.length *= amount;
        p1Paddle.BroadcastRemoteMethod("ModifyLength");
        p2Paddle.BroadcastRemoteMethod("ModifyLength");
    }

    public void ModifySpeed(Paddle p, float amount) {

        //Debug.Log("Modifying the speed of:" + p);
        p.speed *= amount;
        //Debug.Log("Speed is now:" + p.speed);
    }

    public void SetWind(Paddle p, int windsActiveCount) {
        windEffect.SetActive(true);
        if (p == p1Paddle) {
            windDirection = 1;
            windEffect.transform.SetPositionAndRotation(new Vector3(-9.2f, 0, 0), Quaternion.Euler(0, 90f, 0));
        }
        else {
            windDirection = -1;
            windEffect.transform.SetPositionAndRotation(new Vector3(9.2f, 0, 0), Quaternion.Euler(0, -90f, 0));
        }
        windEffect.GetComponent<ParticleSystem>().Play();
    }

    public void DeactivateWind(int timesActivated) {
        bool isActive = timesActivated > 0;
        //windEffect.SetActive(isActive);
    }

    public void FlipControls(Paddle p) {
        p.isInvertedControls = !p.isInvertedControls;
    }

}
