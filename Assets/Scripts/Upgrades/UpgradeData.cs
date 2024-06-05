using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    private int _iconIndex;

    private float _amount;
    private float _duration;

    public enum Type { Buff, Nerf, Neutral };
    public enum Aoe { Self, Other, Both, Field };
    [SerializeField]
    private Type _upgradeType;
    private Aoe _aoe;

    private int _weight;

    public UpgradeData(int id, string name, Type t, Aoe a, float amount, float duration, int weight, int iconIndex) {
        _id = id;
        _name = name;
        _upgradeType = t;
        _aoe = a;
        _amount = amount;
        _duration = duration;
        _iconIndex = iconIndex;
        _weight = weight;
    }

    public Type GetUpgradeType() {
        return _upgradeType;
    }

    public void SetUpgradeType(Type upgradeType) {
        _upgradeType = upgradeType;
    }

    public int GetId() { return _id; }
    public void SetId(int id) { _id = id; }
    public string GetName() { return _name; }
    public void SetName(string name) { _name = name;}

    public int GetIconIndex() { return _iconIndex; }

    public void SetIconIndex(int iconIndex) { _iconIndex = iconIndex; }

    public Aoe GetAoe() { return _aoe;}

    public void SetAoe(Aoe aoe) { _aoe = aoe; }

    public float GetAmount() { return _amount; }

    public void SetAmount(float amount) { _amount = amount; }

    public float GetDuration() { return _duration; }
    public void SetDuration(float duration) { _duration = duration; }

    public int GetWeight() { return _weight;}

    public void SetWeight(int weight) { _weight = weight; }
}
