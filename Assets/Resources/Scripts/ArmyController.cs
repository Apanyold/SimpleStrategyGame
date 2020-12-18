using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public int amrySpeed;

    [HideInInspector]
    public Dictionary<UnitInfo, int> amryInfo;

    private List<UnitInfo> unitInfoList;
    
    private void Start()
    {
        amryInfo = new Dictionary<UnitInfo, int>();
        unitInfoList = GameController.Insnatce.unitsInfo;

        amryInfo.Add(unitInfoList[0], 2);

        UpdateArmyInfo(unitInfoList[0], 2);
    }

    public void UpdateArmyInfo(UnitInfo key, int value)
    {
        if (!amryInfo.TryGetValue(key, out int x))
        {
            amryInfo.Add(key, value);
        }
        else
        {
            amryInfo[key] = x + value;
        }

        Debug.Log("Unit: " + key.name + " Count: " + amryInfo[key]);

        CalculateArmySpeed();
    }

    public void CalculateArmySpeed()
    {
        int min = 100;
        foreach(UnitInfo unit in amryInfo.Keys)
        {
            if (unit.speed < min)
                min = unit.speed;
        }
        amrySpeed = min;
    }
}
