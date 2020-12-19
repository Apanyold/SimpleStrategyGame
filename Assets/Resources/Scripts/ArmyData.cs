using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyData : IComparable<ArmyData>
{
    public ArmyData(UnitInfo unitInfo, int count, int ownerId)
    {
        this.unitInfo = unitInfo;
        this.count = count;
        this.ownerId = ownerId;
    }

    public bool isOnCatle { get; set; }
    public int castleBonusAttack { get; set; }
    public int castleBonusDefence { get; set; }
    public UnitInfo unitInfo { get; set; }
    public int count { get; set; }
    public int ownerId { get; set; }
    public int health
    {
        get
        {
            int C = 0;
            if (isOnCatle)
                C = castleBonusDefence;
            return (unitInfo.defence + C) *count;
        }
    }

    public int strenght
    {
        get
        {
            int C = 0;
            if (isOnCatle)
                C = castleBonusAttack;
            return (unitInfo.attack + C) * count;
        }
    }


    public int CompareTo(ArmyData that)
    {
        if (that == null) return 1;
        if (this.unitInfo.initiative > that.unitInfo.initiative) return -1;
        if (this.unitInfo.initiative < that.unitInfo.initiative) return 1;
        return 0;
    }
}
