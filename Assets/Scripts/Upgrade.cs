using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;

    public enum Type { Buff, Nerf, Neutral};
    public enum Aoe { Self, Other, Both, Field };
    [SerializeField]
    private Type _type;
    private Aoe _aoe;
    
    private Vector3 _position;

    void Start()
    {
        
    }

    public void ColorUpgrade() {
        Color color;
        if (_type == Type.Buff) {
            color = new Color(0.36f, 0.75f, 0.4f);
        }
        else if (_type == Type.Nerf) {
            color = new Color(0.75f, 0.2f, 0.2f);
        }
        else {
            color = new Color(0.75f, 0.65f, 0.2f);
        }
        GetComponent<SpriteRenderer>().color = color;
    }

    public Type GetType() {
        return _type;
    }

    public void SetType(Type type) {
        _type = type;
    }
}
