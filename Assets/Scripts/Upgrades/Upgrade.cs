using Alteruna;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : AttributesSync
{
    [SerializeField]
    private UpgradeData _data;
    private UpgradeManager _upgradeManager;

    [SerializeField]
    private PolygonGenerator _polygonGenerator;

    void Awake()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    [SynchronizableMethod]
    public void ColorUpgrade() {
        //Color color;
        if (_data.GetUpgradeType() == UpgradeData.Type.Buff) {
            //color = new Color(0.36f, 0.75f, 0.4f); //green
            _polygonGenerator.ColorMaterial(0);
        }
        else if (_data.GetUpgradeType() == UpgradeData.Type.Nerf) {
            //color = new Color(0.75f, 0.2f, 0.2f); //red
            _polygonGenerator.ColorMaterial(1);
        }
        else {
            //color = new Color(0.75f, 0.65f, 0.2f);
            _polygonGenerator.ColorMaterial(2);
        }
        //Debug.LogError("Trying to set the icon: " + _data.GetIconIndex());
        //Debug.LogError("Found as: " + _upgradeManager.upgradeIcons[_data.GetIconIndex()]);
        GetComponent<SpriteRenderer>().sprite = _upgradeManager.upgradeIcons[_data.GetIconIndex()];
        //GetComponent<SpriteRenderer>().color = color;
    }

    [SynchronizableMethod]
    public void PickupUpgrade(Ball ball) {
        _upgradeManager.PickupUpgrade(this, ball);
        DestroyUpgrade();
    }

    public void DestroyUpgrade() {
        _upgradeManager.DestroyUpgrade(this);
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Ball")) {
            PickupUpgrade(collision.GetComponent<Ball>());
        }
    }

    public UpgradeData.Type GetUpgradeType() {
        return _data.GetUpgradeType();
    }

    [SynchronizableMethod]
    public void SetUpgradeType(UpgradeData.Type type) {
        _data.SetUpgradeType(type);
    }

    [SynchronizableMethod]
    public void SetData(UpgradeData data) {
        _data = data;
    }

    public UpgradeData GetData() {
        return _data;
    }
}
