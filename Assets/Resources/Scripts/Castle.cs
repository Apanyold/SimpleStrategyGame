using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int
        coinsCurrent,
        poeplesLimit;

    [SerializeField]
    private int peoplesCurrent;
    public int PeoplesCurrent
    {
        get { return peoplesCurrent; }
        set
        {
            if (value > poeplesLimit)
                peoplesCurrent = poeplesLimit;
        }
    }

    public int ownerId;

    [HideInInspector]
    public (int xcord, int ycord) location;


    [HideInInspector]
    public ArmyController castleArmy;

    public List<ArmyController> castleAmryList;
    public List<ArmyData> avaliebleToTrainUnits;
    public List<BuildingsData> buildingsInfo;

    public Buildings buildings;

    public int
        armyDefBonus,
        armyAttackBonus,
        armyTravelDefBonus,
        armyTravelAttackBonus;

    private BuyDelay buyDelay;
    private List<BuyDelay> buyDelayList;

    private void Start()
    {
        coinsCurrent = GameController.Insnatce.startCoins;
        peoplesCurrent = GameController.Insnatce.startPeoples;
        poeplesLimit = GameController.Insnatce.startPeoplesLimit;

        buildingsInfo = new List<BuildingsData>();
        avaliebleToTrainUnits = new List<ArmyData>();
        castleArmy = GetComponent<ArmyController>();
        buyDelayList = new List<BuyDelay>();
        buildings = new Buildings(this);
        foreach (BuildingInfo info in GameController.Insnatce.buildingsInfo)
        {
            BuildingsData temp = new BuildingsData();
            temp.building = info;
            buildingsInfo.Add(temp);
        }
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

    public void OnTurnStart()
    {
        castleAmryList.ForEach(x => x.OnTurnStart());

        buildings.OnTurnStart();

        BuyDelayOnTurn();

        Debug.Log("avaliebleToTrainUnit Castle " + avaliebleToTrainUnits.Count);
    }

    public void MoveArmyFromCastle(List<ArmyData> data)
    {
        List<(int x, int y)> ps = GameController.Insnatce.ArmyMoveZone(gameObject, true);
        ps.Shuffle();
        (int x, int y) position = (0,0);
        Debug.Log("Lenght||||||" + ps.Count);
        foreach ((int x, int y) x in ps) 
        {
            if (GameController.Insnatce.PositionAvailabilityCheck(x))
            {
                position = x;
                break;
            }
        }

        if (position.x != 0 || position.y != 0)
        {
            castleArmy.ArmySplit(data);
            GameController.Insnatce.CreateAmry(data, position, GetComponent<Castle>());
        }
        else
        {
            Debug.LogError("Cannot place army");
        }
    }

    public void BuyUnitWithDelay(UnitInfo info, int count)
    {
        if (info.peopleCost * count > peoplesCurrent || info.coinsCost * count > coinsCurrent)
        {
            Debug.LogError("Not enought");
            return;
        }

        peoplesCurrent -= info.peopleCost * count;
        coinsCurrent -= info.coinsCost * count;

        buyDelay = new BuyDelay(this, new ArmyData(info), count);
    }

    public void BuyDelayOnTurn()
    {
        if (buyDelayList.Count == 0)
            return;

        foreach(BuyDelay delay in buyDelayList.ToArray())
        {
            if (!delay.OnTurn())
            {
                buyDelayList.Remove(delay);
            }
        }
    }

    public void UpdateArmyListy(ArmyController army)
    {
        castleAmryList.Remove(army);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && ownerId == 1)
        {
            //ArmyData a = new ArmyData(GameController.Insnatce.unitsInfo[3], 0, 0);
            ////Debug.Log("CastleArmy");
            ////castleArmy.armyInfo.ForEach(x => Debug.Log(x.unitInfo.name + " " + x.count));
            //Debug.Log(a.UnitTrainTime);
            //a.UnitTrainTime -= 1;
            //Debug.Log(a.UnitTrainTime);

        }
        if (Input.GetMouseButtonDown(2) && ownerId == 1)
        {

            //castleArmy = GetComponent<ArmyController>();
            //castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[0], 1);
            //castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[1], 1);
            //castleArmy.UpdateArmyInfo(GameController.Insnatce.unitsInfo[2], 1);
        }
    }
}