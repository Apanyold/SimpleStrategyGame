using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildings
{
    private Castle castle;

    public enum ACTION {BUY,TURN }

    public Buildings(Castle castle)
    {
        this.castle = castle;
    }

    public void ActionWithBuilding(BuildingsData buildingInfo , ACTION action = ACTION.BUY)
    {
        switch (buildingInfo.building.buidingName)
        {
            case "TownHall":
                {
                    TownHall townHall = new TownHall(buildingInfo, castle);
                    if(action == ACTION.BUY)
                        townHall.OnBuy();
                    else if (action == ACTION.TURN)
                        townHall.OnTurnStart();
                    break;
                }
        }
    }

    public void OnTurnStart()
    {
        foreach(BuildingsData buildingInfo in castle.buildingsInfo)
        {
            ActionWithBuilding(buildingInfo, ACTION.TURN);
        }
    }
}
