using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : IBuilding
{
    private BuildingInfo buildingInfo = GameController.Insnatce.buildingsInfo.Find(x => x.name == "TownHall");

    public void OnBuy()
    {

    }

    public void OnTurnEnd()
    {

    }
}

public class Barracks : IBuilding
{
    private BuildingInfo buildingInfo = GameController.Insnatce.buildingsInfo.Find(x => x.name == "Barracks");

    public void OnBuy()
    {

    }

    public void OnTurnEnd()
    {

    }
}
