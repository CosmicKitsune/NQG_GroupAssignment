using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerStat : MonoBehaviour
{

    public void IncreaseStat(float value)
    {
        float melSpd = PlayerData.meleeSpeed;
        float oldSpd = melSpd;
        melSpd -= value;
        PlayerData.meleeSpeed = melSpd;
        Debug.Log($"Old Spd: {oldSpd} PlayerDataSpd: {PlayerData.meleeSpeed}");
    }
}
