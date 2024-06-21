using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradeData;

public class UpgradeManager : MonoBehaviour {
    [SerializeField]
    private GameManager _gameManager;
    public List<UpgradeData> upgrades;
    private Spawner _spawner;

    public List<GameObject> spawnedUpgrades;
    private int[] _upgradeWeights;

    private const float MIN_X = -5;
    private const float MIN_Y = -4;
    private const float MAX_X = 5;
    private const float MAX_Y = 4;

    private const float initialSpawnUpgradeThreshold = 2f; //for debug make it very low
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

    private bool _debugMode;
    [SerializeField]
    private List<int> _upgradeOccurancesCount;

    void Start() {
        upgradeIndex = 0;
        spawnedUpgrades = new List<GameObject>();
        //PopulateUpgrades();
        _spawner = FindObjectOfType<Spawner>();
        spawnUpgradeThreshold = initialSpawnUpgradeThreshold;
        _upgradeAudioManager = FindObjectOfType<UpgradeAudioManager>();
        _debugMode = false;
        _upgradeWeights = new int[17];
        _upgradeOccurancesCount = new();
        for (int i = 0; i < 17; i++) {
            _upgradeOccurancesCount.Add(0);

        }
        _upgradeOccurancesCount.Add(0); // this will hold the total
    }

    private void FixedUpdate() {
        //disable scripts on the non-host player
        if (!_gameManager.IsGamePlaying())
            return;
        if (windsActiveCount > 0) {
            _balls = FindObjectsOfType<Ball>();
            foreach (var _ball in _balls) {
                _ball.AddForce(new Vector2(windDirection * windsActiveCount * 5, 0));
            }
        }
    }

    private void Initialize() {
        p1Paddle = _gameManager.GetPaddle1();
        p2Paddle = _gameManager.GetPaddle2();
    }

    private void PopulateUpgrades() {
        upgrades = new List<UpgradeData>();

        for (int i = 0; i < _upgradeWeights.Length; i++) {
         //   Debug.Log("Upgrade | index: " + i + ", weight = " + _upgradeWeights[i]);
        }

        //buffs                                                                amount, duration, weight, iconIndex
        UpgradeData longerPaddle = new(101, "Longer Paddle", Type.Buff, Aoe.Self, 1.2f, 3f, _upgradeWeights[0], 1);                     //id = 0
        UpgradeData fasterPaddle = new(102, "Faster Paddle", Type.Buff, Aoe.Self, 1.5f, 8f, _upgradeWeights[1], 2);                     //id = 1
        UpgradeData bonusLife = new(103, "Bonus Life", Type.Buff, Aoe.Self, 1, 0f, _upgradeWeights[2], 3);                              //id = 2
        UpgradeData shorterEnemyPaddle = new(104, "Shorter Enemy Paddle", Type.Buff, Aoe.Other, 0.8f, 3f, _upgradeWeights[3], 11);      //id = 3
        UpgradeData slowerEnemyPaddle = new(105, "Slower Enemy Paddle", Type.Buff, Aoe.Other, 0.7f, 3f, _upgradeWeights[4], 12);        //id = 4
        UpgradeData flipEnemyControls = new(106, "Flip Enemy Controls", Type.Buff, Aoe.Other, 0, 3f, _upgradeWeights[5], 4);            //id = 5
        UpgradeData windBuff = new(107, "Wind With Player", Type.Buff, Aoe.Self, 0, 8f, _upgradeWeights[6], 5);                         //id = 6

        upgrades.Add(longerPaddle);
        upgrades.Add(fasterPaddle);
        upgrades.Add(bonusLife);
        upgrades.Add(shorterEnemyPaddle);
        upgrades.Add(slowerEnemyPaddle);
        upgrades.Add(flipEnemyControls);
        upgrades.Add(windBuff);

        //nerfs                                                                     //amount, duration, weight, iconIndex
        UpgradeData shorterPaddle = new(201, "Shorter Paddle", Type.Nerf, Aoe.Self, 0.8f, 3f, _upgradeWeights[7], 11);                   //id = 7
        UpgradeData slowerPaddle = new(202, "Slower Paddle", Type.Nerf, Aoe.Self, 0.7f, 3f, _upgradeWeights[8], 12);                     //id = 8
        UpgradeData flipControls = new(203, "Flip Controls", Type.Nerf, Aoe.Self, 0, 3f, _upgradeWeights[9], 4);                         //id = 9
        UpgradeData windNerf = new(204, "Wind Against Player", Type.Nerf, Aoe.Other, 0, 8f, _upgradeWeights[10], 5);                      //id = 10

        upgrades.Add(shorterPaddle);
        upgrades.Add(slowerPaddle);
        upgrades.Add(flipControls);
        upgrades.Add(windNerf);

        //neutrals
        UpgradeData shorterBothPaddles = new(301, "Shorter Both Paddles", Type.Neutral, Aoe.Both, 0.8f, 3f, _upgradeWeights[11], 11);     //id = 11
        UpgradeData longerBothPaddles = new(302, "Longer Both Paddles", Type.Neutral, Aoe.Both, 1.2f, 3f, _upgradeWeights[12], 1);        //id = 12
        UpgradeData fasterBothPaddles = new(303, "Faster Both Paddles", Type.Neutral, Aoe.Both, 1.4f, 3f, _upgradeWeights[13], 2);          //id = 13
        UpgradeData slowerBothPaddles = new(304, "Slower Both Paddles", Type.Neutral, Aoe.Both, 0.7f, 3f, _upgradeWeights[14], 12);       //id = 14
        UpgradeData flipBothControls = new(305, "Flip Both Controls", Type.Neutral, Aoe.Both, 0, 3f, _upgradeWeights[15], 4);             //id = 15
        UpgradeData splitBall = new(306, "Split Ball", Type.Neutral, Aoe.Both, 0, 0, _upgradeWeights[16], 21);                           //id = 16

        upgrades.Add(shorterBothPaddles);
        upgrades.Add(longerBothPaddles);
        upgrades.Add(fasterBothPaddles);
        upgrades.Add(slowerBothPaddles);
        upgrades.Add(flipBothControls);
        upgrades.Add(splitBall);

    }

    private void Update() {
        if (!_gameManager.IsGamePlaying())
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            upgradeIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            upgradeIndex = 1;
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
        CleanSpawnedUpgrades();
    }
    // ticks the timer and spawns random upgrades at a set interval
    public void CheckSpawnUpgrade() {
        timeSinceLastSpawnedUpgrade += Time.deltaTime;

        if (timeSinceLastSpawnedUpgrade >= spawnUpgradeThreshold && !_debugMode) {
            //randomize and announce an index
            upgradeIndex = GenerateRandomUpgradeIndex();
            if (upgradeIndex != -1) {
                SpawnUpgrade();
                timeSinceLastSpawnedUpgrade = 0;
            }
        }
    }

    public void SpawnUpgrade() {

        float xCoord = 0;
        float yCoord = 0;
        Vector3 pos;
        bool validPosition = false;
        int attempts = 0;
        while (!validPosition && attempts < 50) {
            validPosition = true;
            //xCoord = _debugMode ? 0 : Random.Range(MIN_X, MAX_X);
            xCoord = Random.Range(MIN_X, MAX_X);
            yCoord = Random.Range(MIN_Y, MAX_Y);
            pos = new(xCoord, yCoord, 0f);
            foreach (GameObject spawnedUpgradeGO in spawnedUpgrades) {
                if (spawnedUpgradeGO != null) {
                    float distance = Vector3.Distance(pos, spawnedUpgradeGO.transform.position);
                    // if the distance is less than the sum of the radii of both upgrades, then it's an invalid position
                    if (distance < 0.6f + 0.6f) {
                        validPosition = false;
                        break;
                    }
                }
            }
            attempts++;
        }
        if (validPosition) {

            pos = new(xCoord, yCoord, 0f);
            GameObject upgradeGO = _spawner.Spawn(1, pos, Quaternion.identity, new Vector3(1f, 1f, 1f));
            spawnedUpgrades.Add(upgradeGO);

            Upgrade upgrade = upgradeGO.GetComponent<Upgrade>();
            UpgradeData upgradeData = upgrades[upgradeIndex];
            upgrade.BroadcastRemoteMethod(nameof(upgrade.SetData), upgradeData);
            upgrade.BroadcastRemoteMethod(nameof(upgrade.ColorUpgrade));
        }
        else {
            Debug.LogError("No valid positions for the upgrade");
        }
    }

    private void CleanSpawnedUpgrades() {
        for (int i = 0; i < spawnedUpgrades.Count; i++) {
            if (spawnedUpgrades[i] == null) {
                spawnedUpgrades.RemoveAt(i);
            }
        }
    }

    public int GenerateRandomUpgradeIndex() {
        // use the upgrades' weights to return a weighted random choice of an upgrade
        if (totalWeight == 0) {
            foreach (var upgrade in upgrades) {
                totalWeight += upgrade.GetWeight();
            }
            //Debug.Log("Total weight is: " + totalWeight);
        }
        int randIndex = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        for (int i = 0; i < upgrades.Count; i++) {
            cumulativeWeight += upgrades[i].GetWeight();
            if (randIndex < cumulativeWeight) {
                //Debug.Log("Random upgrade chosen has index: " + i); //useful when debugging to check prob distribution is working nicely
                //better to debug this way:
                _upgradeOccurancesCount[i]++;
                _upgradeOccurancesCount[_upgradeOccurancesCount.Count - 1]++;
                return i;
            }
        }

        return -1;
    }


    public void SetUpgradeWeights(int[] weights) {
        _upgradeWeights = weights;
        PopulateUpgrades();
    }

    public void PickupUpgrade(Upgrade upgrade, Ball ball) {
        //disable on non-host player
        if (!_gameManager.IsGamePlaying())
            return;

        if (p1Paddle == null || p2Paddle == null) {
            Initialize();
        }

        spawnedUpgrades.Remove(upgrade.gameObject);

        //play the appropriate sound
        _upgradeAudioManager.OnCollectUpgrade(upgrade);

        string upgradeName = upgrade.GetData().GetName();
        UpgradeData.Aoe aoe = upgrade.GetData().GetAoe();
        float amount = upgrade.GetData().GetAmount();
        float duration = upgrade.GetData().GetDuration();

        Paddle targetPaddle = GetTargetPaddle(aoe, ball);

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

            case "Split Ball":
                SplitBall(ball);
                break;

            default:
                Debug.LogError("Unknown Upgrade has been picked up");
                Debug.LogError("Its name is: " + upgradeName);
                Debug.LogError("Its ID is: " + upgrade.GetData().GetId());
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
    }

    private Paddle GetTargetPaddle(Aoe aoe, Ball ball) {
        // If aoe is self, we keep the targetIndex as the paddle that touched the ball last
        // If aoe is other, we swap by doing (1 - index)
        int ballTouchedBy = ball.GetLastTouchedBy();
        int targetIndex = (aoe == Aoe.Self) ? ballTouchedBy : (1 - ballTouchedBy);
        return targetIndex == 0 ? p1Paddle : p2Paddle;
    }

    public void ModifyLength(Paddle p, float amount) {

        p.length *= amount;
        p1Paddle.BroadcastRemoteMethod(nameof(Paddle.ModifyLength));
        p2Paddle.BroadcastRemoteMethod(nameof(p2Paddle.ModifyLength));
    }

    public void ModifySpeed(Paddle p, float amount) {

        //Debug.Log("Modifying the speed of:" + p);
        p.speed *= amount;
        //Debug.Log("Speed is now:" + p.speed);
    }


    public void SetWind(Paddle p, int windsActiveCount) {
        windEffect.SetActive(true);
        windDirection = p.transform.position.x < 0 ? 1 : -1;
        p1Paddle.BroadcastRemoteMethod(nameof(p1Paddle.BlowWind), windDirection == 1);
        p2Paddle.BroadcastRemoteMethod(nameof(p2Paddle.BlowWind), windDirection == 1);
    }

    public void SplitBall(Ball ball) {
        Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();

        float offset = ballRB.velocity.x > 0 ? -0.5f : 0.5f;

        Vector3 ballPosition = ball.transform.position + new Vector3(offset, 0, 0);
        //Spawn a ball
        GameObject newBallGO = _spawner.Spawn(0, ballPosition, Quaternion.identity, new Vector3(0.3f, 0.3f, 1f));
        newBallGO.GetComponent<Rigidbody2DSynchronizable>().enabled = enabled;
        
        Ball newBall = newBallGO.GetComponent<Ball>();
        newBall.BroadcastRemoteMethod(nameof(newBall.SetBallSkin), ball.skinIndex);

        //ensure that the new ball moves away from the old ball
        float x = ballRB.velocity.x > 0 ? -1.0f : 1.0f;
        float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        Vector2 direction = new(x, y);
        newBallGO.GetComponent<Rigidbody2D>().AddForce(direction * ball.speed);

    }

    public void FlipControls(Paddle p) {
        p.isInvertedControls = !p.isInvertedControls;
    }

}
