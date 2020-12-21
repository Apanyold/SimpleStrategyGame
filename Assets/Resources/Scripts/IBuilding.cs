using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    void OnTurnStart();
    void OnBuy();
}


public class TownHall : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;
    private float tax = 0.1f;

    public TownHall(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;

    }

    public void OnTurnStart()
    {
        for (int i = 1; i < buildingInfo.currentLevel && i <= buildingInfo.building.maxLevel; i++)
            tax += 0.02f;
        float taxIncome = castle.peoplesCurrent * tax;
        castle.coinsCurrent += (int)taxIncome;
    }

    public void OnBuy()
    {
        Debug.Log(buildingInfo.upgrateCost);
        if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
        }
    }
}

//public class Barracks : IBuilding
//{
//    public BuildingInfo buildingInfo = GameController.Insnatce.buildingsInfo.Find(x => x.name == "Barracks");


//}