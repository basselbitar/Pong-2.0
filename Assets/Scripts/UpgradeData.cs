using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeData
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;

    public enum Type { Buff, Nerf, Neutral };
    public enum Aoe { Self, Other, Both, Field };
    [SerializeField]
    private Type _upgradeType;
    private Aoe _aoe;

    private Vector3 _position;

    public Type GetUpgradeType() {
        return _upgradeType;
    }

    public void SetUpgradeType(Type upgradeType) {
        _upgradeType = upgradeType;
    }
}
