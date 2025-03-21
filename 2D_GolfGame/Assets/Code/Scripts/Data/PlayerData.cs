using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Attributes")]
    public static float maxPower = 7f; //max power applied
    public static float power = 2f; //power applied
    public static float maxGoalSpeed = 4f;
    public static float maxVelocity = 10f;
    public static int maxBounceCount = 3;
    public static float meleeSpeed = 0.3f;
    public static float damage = 1f;
}
