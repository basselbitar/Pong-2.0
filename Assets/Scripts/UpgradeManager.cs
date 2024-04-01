using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> Upgrades;
    private Spawner _spawner;

    private const float MIN_X = -5;
    private const float MIN_Y = -4;
    private const float MAX_X = 5;
    private const float MAX_Y = 4;

    void Start()
    {
        PopulateUpgrades();
        _spawner = FindObjectOfType<Spawner>();
    }

    private void PopulateUpgrades() {
        Upgrades = new List<Upgrade>();
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            SpawnUpgrade();
        }
    }

    public void SpawnUpgrade() {
        Vector3 pos = new Vector3(Random.Range(MIN_X, MAX_X), Random.Range(MIN_Y, MAX_Y), 0f);
        GameObject upgradeGO = _spawner.Spawn(1, pos, Quaternion.identity, new Vector3(1f, 1f, 1f));

        Upgrade upgrade = upgradeGO.GetComponent<Upgrade>();

        upgrade.BroadcastRemoteMethod("SetUpgradeType", (UpgradeData.Type)Random.Range(0, 3));
        upgrade.BroadcastRemoteMethod("ColorUpgrade");

    }

    public void DestroyUpgrade(Upgrade upgrade) {
        _spawner.Despawn(upgrade.gameObject);
    }

}
