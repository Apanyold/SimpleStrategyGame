using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDelay
{
    Castle castle;
    ArmyData unit;
    int count;

    public BuyDelay(Castle castle, ArmyData unit, int count)
    {
        this.castle = castle;
        this.unit = unit;
        this.count = count;
    }

    public bool OnTurn()
    {
        Debug.Log("Hello");
        if (unit.UnitTrainTime > 0)
        {
            Debug.Log("unit.UnitTrainTime " + unit.UnitTrainTime);
            unit.UnitTrainTime--;
            return false;
        }
        else
        {
            castle.castleArmy.UpdateArmyInfo(unit.unitInfo, count);
            Debug.Log("unit.unitInfo.name " + unit.unitInfo.name + " " + count);
            return true;
        }
    }
}
