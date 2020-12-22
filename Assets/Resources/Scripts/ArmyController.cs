using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public int ownerId;

    [HideInInspector]
    public int amrySpeed;

    [HideInInspector]
    public List<ArmyData> armyInfo;
    private List<UnitInfo> unitInfoList;

    private Grid mainGrid;

    public bool 
        isMovedThisTurn,
        isOnCastle;

    [HideInInspector]
    public int
        armyDefBonus,
        armyAttackBonus;

    public int armyTravelDefBonus { get => homeCastle.armyTravelDefBonus; }

    public int armyTravelAttackBonus { get => homeCastle.armyTravelAttackBonus; }

    private Castle homeCastle;

    private int carriedCoins;
    public int CarriedCoins
    {
        get
        {
            return carriedCoins;
        }
        set
        {
            Debug.Log("carriedCoins: " + carriedCoins + " value: " + value);
            carriedCoins = value;
        }
    }
    #endregion
    
    private void Start()
    {
        unitInfoList = GameController.Insnatce.unitsInfo;

        //testing field;
        //Debug.Log("ArmyController start with owner ID: " + ownerId);
        //if (ownerId != 1)
        //{
        //    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        //    Debug.Log("Army with id 1 start");
        //    UpdateArmyInfo(unitInfoList[0], 1);
        //    UpdateArmyInfo(unitInfoList[1], 1);
        //    UpdateArmyInfo(unitInfoList[2], 1);
        //}
        //

        armyInfo = new List<ArmyData>();
        if (TryGetComponent(out Castle C))
        {
            UpdateCatleArmyInfo();
            isOnCastle = true;
            isMovedThisTurn = true;
            ownerId = C.ownerId;

            armyDefBonus = C.armyDefBonus;
            armyAttackBonus = C.armyAttackBonus;
        }
        mainGrid = GameController.Insnatce.grid;
    }

    public void OnTurnStart()
    {
        if (!isOnCastle)
        {
            isMovedThisTurn = false;
        }
    }

    public void InitArmy(List<ArmyData> army, Castle castle)
    {
        armyInfo = army;
        ownerId = armyInfo[0].ownerId;
        isMovedThisTurn = true;
        homeCastle = castle;

        if (ownerId == 1)
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        else
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        FindObjectOfType<Castle>();

        CalculateArmySpeed();
    }

    public void UpdateArmyInfo(UnitInfo unitInfo, int count)
    {
        //Debug.Log("Unity name: " + unitInfo.name + "| unit count: " + count);
        if (armyInfo.Count != 0 && armyInfo.Exists(x => x.unitInfo.name == unitInfo.name))
        {
            armyInfo.Find(x => x.unitInfo.name == unitInfo.name).count += count;
        }
        else
            armyInfo.Add(new ArmyData(unitInfo, count, ownerId));

        //armyInfo.ForEach(x => Debug.Log("Unity name: " + x.unitInfo.name + " count: " + x.count));

        CalculateArmySpeed();
    }

    public void ArmySplit(List<ArmyData> data)
    {
        foreach (ArmyData army in armyInfo.ToArray())
        {
            if (army.count <= 0)
                continue;
            if(data.Exists(x => x.unitInfo.name == army.unitInfo.name))
                army.count -= data.Find(x => x.unitInfo.name == army.unitInfo.name).count;

            if (army.count <= 0)
            {
                armyInfo.Remove(army);
            }
        }
    }

    public void UpdateArmyInfo(List<ArmyData> armyDatas, bool isClearArmyData = true)
    {
        if(isClearArmyData)
            armyInfo = new List<ArmyData>();

        foreach(ArmyData amry in armyDatas)
        {
            UpdateArmyInfo(amry.unitInfo, amry.count);
        }
    }

    public void CalculateArmySpeed()
    {
        if(armyInfo.Count > 0)
            amrySpeed = armyInfo.Min(x => x.unitInfo.speed);
        else
        {
            amrySpeed = 0;
            Debug.Log("Army speed is 0, is something wrong?");
        }

    }

    public void OnTurnEnd()
    {
        if (!isOnCastle)
            isMovedThisTurn = false;
    }

    public void UpdateCatleArmyInfo()
    {
        armyInfo.ForEach(x => x.castleBonusAttack = armyAttackBonus);
        armyInfo.ForEach(x => x.castleBonusDefence = armyDefBonus);
        armyInfo.ForEach(x => x.travelBonusAttack = armyTravelAttackBonus);
        armyInfo.ForEach(x => x.travelBonusDefence = armyTravelDefBonus);
    }

    public void MoveArmyTo(Vector3 position)
    {
        if (isMovedThisTurn || isOnCastle)
        {
            return;
        }

        GameObject pointObject = mainGrid.GetValue(position);

        if (pointObject == null)
        {
            Debug.Log("Move to empty point");
        }
        else if(pointObject.TryGetComponent(out Castle castle) && castle.ownerId == ownerId)
        {
            if(castle.TryGetComponent(out ArmyController castleArmy))
            {
                castleArmy.UpdateArmyInfo(armyInfo, false);
                castle.coinsCurrent += CarriedCoins;
            }
        }
        else if (pointObject.TryGetComponent(out ArmyController otherArmy))
        {
            if (otherArmy.ownerId == ownerId)
            {
                Debug.Log("Move to own Army");
            }
            else if (otherArmy.ownerId != ownerId)
            {
                Debug.Log("Move to enemy Army");
                UpdateCatleArmyInfo();
                GameController.Insnatce.fightController.InitiateFight(this, otherArmy);
            }
        }

        isMovedThisTurn = true;

        mainGrid.MoveTo(position, gameObject);
    }

    public void Die(ArmyController armyKiller)
    {
        Debug.Log("Army owned by:" + ownerId + " died");
        if (isOnCastle)
        {
            if (TryGetComponent(out Castle c))
            {
                if(c.coinsCurrent > 0)
                    armyKiller.CarriedCoins += c.coinsCurrent / 2;
                c.CastleDestroy();
            }
        }
        else
            Destroy(gameObject);
    }
}
