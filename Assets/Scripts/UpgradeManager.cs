using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradeData;

public class UpgradeManager : MonoBehaviour
{
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

    void Start()
    {
        PopulateUpgrades();
        _spawner = FindObjectOfType<Spawner>();
    }

    private void PopulateUpgrades() {
        upgrades = new List<UpgradeData>();

        UpgradeData longerPaddle = new(101, "Longer Paddle", Type.Buff, Aoe.Self, 2f, 3f);
        UpgradeData fasterPaddle = new(102, "Faster Paddle", Type.Buff, Aoe.Self, 2f, 3f);

        UpgradeData shorterEnemyPaddle = new(201, "Shorter Enemy Paddle", Type.Nerf, Aoe.Other, 0.5f, 3f);
        UpgradeData slowerEnemyPaddle = new(202, "Slower Enemy Paddle", Type.Nerf, Aoe.Other, 0.5f, 3f);

        upgrades.Add(longerPaddle);
        upgrades.Add(fasterPaddle);
        upgrades.Add(shorterEnemyPaddle);
        upgrades.Add(slowerEnemyPaddle);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            SpawnUpgrade();
        }
    }

    public void SpawnUpgrade() {
        Vector3 pos = new(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0f);
        GameObject upgradeGO = _spawner.Spawn(1, pos, Quaternion.identity, new Vector3(1f, 1f, 1f));

        Upgrade upgrade = upgradeGO.GetComponent<Upgrade>();
        //UpgradeData upgradeData = upgrades[Random.Range(0, upgrades.Count)]; //TODO: make upgrades not equally probably from each other
        UpgradeData upgradeData = upgrades[0]; //TODO: make upgrades not equally probably from each other

        upgrade.BroadcastRemoteMethod("SetData", upgradeData);

        upgrade.BroadcastRemoteMethod("ColorUpgrade");

    }

    private void Initialize() {
        p1Paddle = _gameManager.GetPaddle1();
        p2Paddle = _gameManager.GetPaddle2();
    }

    public void PickupUpgrade(Upgrade upgrade) {
        if(p1Paddle == null || p2Paddle == null)
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
                //_gameManager.ModifySpeed(aoe, amount);
                break;
            case "Shorter Enemy Paddle":
                //_gameManager.ModifyLength(aoe, amount);
                break;
            case "Slower Enemy Paddle":
                //_gameManager.ModifySpeed(aoe, amount);
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
        ModifyLength(p, 1f/amount);
    }

    private Paddle GetTargetPaddle(Aoe aoe) {
        // If aoe is self, we keep the targetIndex as the paddle that touched the ball last
        // If aoe is other, we swap by doing (1 - index)
        int ballTouchedBy = _gameManager.GetBallTouchedBy();
        int targetIndex = (aoe == Aoe.Self) ? ballTouchedBy : (1 - ballTouchedBy);
        return targetIndex == 0 ? p1Paddle : p2Paddle;
    }

    public void ModifyLength(Paddle p, float amount) {
        
        Debug.Log("Modifying the length of:" + p);
        p.length *= amount;
        p1Paddle.BroadcastRemoteMethod("ModifyLength"); // applies it to both paddles
        p2Paddle.BroadcastRemoteMethod("ModifyLength"); // applies it to both paddles
    }

    public void ModifySpeed(Paddle p, float amount) {

        Debug.Log("Modifying the speed of:" + p);
        p.speed *= amount;
    }

}
