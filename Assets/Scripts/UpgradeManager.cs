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

    private const float MIN_X = -5;
    private const float MIN_Y = -4;
    private const float MAX_X = 5;
    private const float MAX_Y = 4;

    private Paddle p1Paddle;
    private Paddle p2Paddle;

    public Sprite[] upgradeIcons;

    [SerializeField]
    public int upgradeIndex; //debug purposes

    void Start() {
        upgradeIndex = 0;
        PopulateUpgrades();
        _spawner = FindObjectOfType<Spawner>();
    }

    private void PopulateUpgrades() {
        upgrades = new List<UpgradeData>();

        UpgradeData longerPaddle = new(101, "Longer Paddle", Type.Buff, Aoe.Self, 1.2f, 3f, 1);
        UpgradeData fasterPaddle = new(102, "Faster Paddle", Type.Buff, Aoe.Self, 4f, 8f, 2);

        UpgradeData shorterEnemyPaddle = new(201, "Shorter Enemy Paddle", Type.Nerf, Aoe.Other, 0.5f, 3f, 3);
        UpgradeData slowerEnemyPaddle = new(202, "Slower Enemy Paddle", Type.Nerf, Aoe.Other, 0.5f, 3f, 4);

        UpgradeData shorterBothPaddles = new(301, "Shorter Both Paddles", Type.Neutral, Aoe.Both, 0.5f, 3f, 0);
        UpgradeData longerBothPaddles = new(302, "Longer Both Paddles", Type.Neutral, Aoe.Both, 1.5f, 3f, 0);
        UpgradeData fasterBothPaddles = new(303, "Faster Both Paddles", Type.Neutral, Aoe.Both, 3f, 3f, 0);
        UpgradeData slowerBothPaddles = new(304, "Slower Both Paddles", Type.Neutral, Aoe.Both, 0.1f, 3f, 0);

        //buffs
        upgrades.Add(longerPaddle);
        upgrades.Add(fasterPaddle);

        //nerfs
        upgrades.Add(shorterEnemyPaddle);
        upgrades.Add(slowerEnemyPaddle);

        //neutrals
        upgrades.Add(shorterBothPaddles);
        upgrades.Add(longerBothPaddles);
        upgrades.Add(fasterBothPaddles);
        upgrades.Add(slowerBothPaddles);

    }

    private void Update() {
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

        if (Input.GetKeyDown(KeyCode.J)) {
            SpawnUpgrade();
        }

    }

    public void SpawnUpgrade() {
        Vector3 pos = new(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0f);
        GameObject upgradeGO = _spawner.Spawn(1, pos, Quaternion.identity, new Vector3(1f, 1f, 1f));

        Upgrade upgrade = upgradeGO.GetComponent<Upgrade>();
        //UpgradeData upgradeData = upgrades[Random.Range(0, upgrades.Count)]; //TODO: make upgrades not equally probably from each other
        Debug.Log(upgradeIndex);
        UpgradeData upgradeData = upgrades[upgradeIndex]; //TODO: make upgrades not equally probably from each other

        upgrade.BroadcastRemoteMethod("SetData", upgradeData);

        upgrade.BroadcastRemoteMethod("ColorUpgrade");

    }

    private void Initialize() {
        p1Paddle = _gameManager.GetPaddle1();
        p2Paddle = _gameManager.GetPaddle2();
    }

    public void PickupUpgrade(Upgrade upgrade) {
        if (p1Paddle == null || p2Paddle == null)
            Initialize();

        string upgradeName = upgrade.GetData().GetName();
        UpgradeData.Aoe aoe = upgrade.GetData().GetAoe();
        float amount = upgrade.GetData().GetAmount();
        float duration = upgrade.GetData().GetDuration();

        Paddle targetPaddle = GetTargetPaddle(aoe);
        //start the timer on the picked up upgrade

        switch (upgradeName) {
            case "Longer Paddle":
                StartCoroutine(ModifyLength(targetPaddle, amount, duration));
                break;
            case "Faster Paddle":
                StartCoroutine(ModifySpeed(targetPaddle, amount, duration));
                break;
            case "Shorter Enemy Paddle":
                StartCoroutine(ModifyLength(targetPaddle, amount, duration));
                break;
            case "Slower Enemy Paddle":
                StartCoroutine(ModifySpeed(targetPaddle, amount, duration));
                break;
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

        Debug.Log("Modifying the speed of:" + p);
        p.speed *= amount;
        Debug.Log("Speed is now:" + p.speed);
    }

}
