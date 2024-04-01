using Alteruna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : AttributesSync
{
    private UpgradeData _data;
    private UpgradeManager _upgradeManager;

    void Start()
    {
        
        _upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    [SynchronizableMethod]
    public void ColorUpgrade() {
        Color color;
        if (_data.GetUpgradeType() == UpgradeData.Type.Buff) {
            color = new Color(0.36f, 0.75f, 0.4f);
        }
        else if (_data.GetUpgradeType() == UpgradeData.Type.Nerf) {
            color = new Color(0.75f, 0.2f, 0.2f);
        }
        else {
            color = new Color(0.75f, 0.65f, 0.2f);
        }
        GetComponent<SpriteRenderer>().color = color;
    }

    [SynchronizableMethod]
    public void PickupUpgrade() {
        DestroyUpgrade();
    }

    private void DestroyUpgrade() {
        _upgradeManager.DestroyUpgrade(this);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ball")) {
            PickupUpgrade();
        }
    }

    public UpgradeData.Type GetUpgradeType() {
        return _data.GetUpgradeType();
    }

    [SynchronizableMethod]
    public void SetUpgradeType(UpgradeData.Type type) {
        _data = new UpgradeData();
        _data.SetUpgradeType(type);
    }
}
