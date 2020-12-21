using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int 
        coinsCurrent,
        peoplesCurrent,
        coinsPerTurn,
        poeplesPerTurn;

    [HideInInspector]
    public int ownerId;

    [HideInInspector]
    public (int xcord, int ycord) location;

    public List<BuildingsData> buildingsInfo;

    [HideInInspector]
    public ArmyController castleArmy;

    //List<(int x, int y)>

    private void Start()
    {
        coinsCurrent = GameController.Insnatce.startCoins;
        peoplesCurrent = GameController.Insnatce.startPeoples;
        buildingsInfo = new List<BuildingsData>();

        foreach (BuildingInfo info in GameController.Insnatce.buildingsInfo)
        {
            BuildingsData temp = new BuildingsData();
            temp.building = info;
            buildingsInfo.Add(temp);
        }

        if(ownerId == 1)
        {
            castleArmy = GetComponent<ArmyController>();
            castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[0], 1);
            castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[1], 1);
            castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[2], 1);
        }
    }
    
    public void MoveArmyFromCastle(List<ArmyData> army)
    {
        List<(int x, int y)> moveZone = GameController.Insnatce.ArmyMoveZone(gameObject, true);

    }

    private void OnDestroy()
    {
        if(gameObject.GetComponent<PlayerController>() != null)
        {
            //Debug.Log("Game over. Catle message");
        }
        else
        {
            //Debug.Log("Player gets 50% of destroyed castle gold");
        }
    }

    public void CastleDestroy()
    {

    }

    public void OnTurn()
    {

    }
}