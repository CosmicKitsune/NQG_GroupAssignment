using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class Weapon : ScriptableObject {
    public new string name;
    public string description;
    public Sprite icon;
    public float meleeSpeed;
    public float damage;
    public float range;
    public float chargeTime;
    public int atkTimes;
}
