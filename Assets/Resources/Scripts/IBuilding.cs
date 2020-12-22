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
        for (int i = 1; i < buildingInfo.currentLevel; i++)
            tax += 0.02f;
        float taxIncome = castle.PeoplesCurrent * tax;
        castle.coinsCurrent += (int)taxIncome;
    }

    public void OnBuy()
    {
        Debug.Log(buildingInfo.upgrateCost);
        if(buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
        }
    }
}

public class House : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;
    private int bonusPerLvl = 200;

    public House(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;

        castle.poeplesLimit += bonusPerLvl;
    }

    public void OnBuy()
    {
        Debug.Log(buildingInfo.upgrateCost);
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.poeplesLimit += bonusPerLvl;

            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
        }
    }

    public void OnTurnStart()
    {
        if(castle.PeoplesCurrent <= castle.poeplesLimit)
            castle.PeoplesCurrent += buildingInfo.currentLevel * 10;
    }
}

public class Wall : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Wall(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            if (buildingInfo.currentLevel % 2 == 0)
            {
                castle.armyDefBonus++;
            }
        }
    }

    public void OnTurnStart()
    {

    }
}

public class Temple : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Temple(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            if (buildingInfo.currentLevel % 3 == 0)
            {
                castle.armyTravelAttackBonus++;
            }
            if (buildingInfo.currentLevel % 2 == 0)
            {
                castle.armyAttackBonus++;
            }
        }
    }

    public void OnTurnStart()
    {

    }
}

public class Barracks : IBuilding
{
    private BuildingsData buildingInfo;
    private Castle castle;

    public Barracks(BuildingsData buildingInfo, Castle castle)
    {
        this.buildingInfo = buildingInfo;
        this.castle = castle;
        CheckPermissionAndBonuses();
    }

    public void OnBuy()
    {
        if (buildingInfo.currentLevel >= buildingInfo.building.maxLevel)
        {
            Debug.LogError("Max level");
        }
        else if (castle.coinsCurrent >= buildingInfo.upgrateCost)
        {
            castle.coinsCurrent -= buildingInfo.upgrateCost;
            buildingInfo.currentLevel++;
            CheckPermissionAndBonuses();
        }
    }

    public void CheckPermissionAndBonuses()
    {
        ArmyData data = new ArmyData(null, 0, castle.ownerId); ;
        if (buildingInfo.currentLevel == 1)
        {
            if (!castle.avaliebleToTrainUnits.Exists(x => x.unitInfo.unitName == "Archer"))
            {
                data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Archer");
                castle.avaliebleToTrainUnits.Add(data);
            }
        }
        if (buildingInfo.currentLevel == 2 || buildingInfo.currentLevel == 3)
        {
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[0] = data;
        }
        if (buildingInfo.currentLevel == 4)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Footman");
            castle.avaliebleToTrainUnits[1] = data;
        }
        if (buildingInfo.currentLevel == 5 || buildingInfo.currentLevel == 6)
        {
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[1] = data;
        }
        if (buildingInfo.currentLevel == 7)
        {
            data.unitInfo = GameController.Insnatce.unitsInfo.Find(x => x.unitName == "Rider");
            castle.avaliebleToTrainUnits[2] = data;
        }
        if (buildingInfo.currentLevel == 8 || buildingInfo.currentLevel == 9 || buildingInfo.currentLevel == 10)
        {
            data.UnitTrainTime--;
            castle.avaliebleToTrainUnits[2] = data;
        }

    }

    public void OnTurnStart()
    {

    }
}