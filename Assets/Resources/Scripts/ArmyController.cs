using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    #region Variables
    [HideInInspector]
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

    public int carriedCoins
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
    private void Awake()
    {
        armyInfo = new List<ArmyData>();
    }
    private void Start()
    {

        unitInfoList = GameController.Insnatce.unitsInfo;

        //testing field;
        //Debug.Log("ArmyController start with owner ID: " + ownerId);
        if (ownerId != 1)
        {
            Debug.Log("Army with id 1 start");
            UpdateArmyInfo(unitInfoList[0], 1);
            UpdateArmyInfo(unitInfoList[1], 1);
            UpdateArmyInfo(unitInfoList[2], 1);
        }
        //
        if (TryGetComponent(out Castle C))
        {
            UpdateCatleArmyInfo();
            isOnCastle = true;
            isMovedThisTurn = true;
            ownerId = C.ownerId;
        }
        mainGrid = GameController.Insnatce.grid;

        if(!isOnCastle)
            isMovedThisTurn = false;
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
        armyInfo.ForEach(x => x.isOnCatle = true);
        armyInfo.ForEach(x => x.castleBonusAttack = armyAttackBonus);
        armyInfo.ForEach(x => x.castleBonusDefence = armyDefBonus);
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
                castle.coinsCurrent += carriedCoins;
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
                    armyKiller.carriedCoins += c.coinsCurrent / 2;
                c.CastleDestroy();
            }
        }
        else
            Destroy(gameObject);
    }
}
