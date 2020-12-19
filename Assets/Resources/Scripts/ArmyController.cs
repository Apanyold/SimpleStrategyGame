using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
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
        ifOnCastle;

    [HideInInspector]
    public int
        armyDefBonus,
        armyAttackBonus;


    //testing field;
    List<ArmyData> enemyArmyInfoTest;
    private void Awake()
    {
        armyInfo = new List<ArmyData>();
    }
    private void Start()
    {

        unitInfoList = GameController.Insnatce.unitsInfo;

        //testing field;
        Debug.Log("ArmyController start with owner ID: " + ownerId);
        if (ownerId != 1)
        {
            UpdateArmyInfo(unitInfoList[0], 1);
            UpdateArmyInfo(unitInfoList[1], 1);
            UpdateArmyInfo(unitInfoList[2], 1);
        }
        //
        if (TryGetComponent(out Castle C))
        {
            UpdateCatleArmyInfo();
            ifOnCastle = true;
            isMovedThisTurn = true;
            ownerId = C.ownerId;
        }


        //GameController.Insnatce.fightController.InitiateFight(armyInfo, enemyArmyInfoTest);

        //CreateAttackOrder(enemyArmyInfoTest);

        mainGrid = GameController.Insnatce.grid;

        if(!ifOnCastle)
            isMovedThisTurn = false;
    }

    public void UpdateArmyInfo(UnitInfo unitInfo, int count)
    {
        Debug.Log("Unity name: " + unitInfo.name + armyInfo.Count);
        if (armyInfo.Count != 0 && armyInfo.Exists(x => x.unitInfo.name == unitInfo.name))
        {
            armyInfo.Find(x => x.unitInfo.name == unitInfo.name).count += count;
        }
        else
            armyInfo.Add(new ArmyData(unitInfo, count, ownerId));

        //armyInfo.ForEach(x => Debug.Log("Unity name: " + x.unitInfo.name + " count: " + x.count));

        CalculateArmySpeed();
    }

    public void NewArmyInfo(List<ArmyData> armyDatas)
    {
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
        if (!ifOnCastle)
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
        if (isMovedThisTurn)
        {
            UiController.Instance.ShowNotification("Your army has already moved on this turn");
            return;
        }

        GameObject pointObject = mainGrid.GetValue(position);

        if (pointObject == null)
        {
            Debug.Log("Move to empty point");
        }
        else if (pointObject.TryGetComponent(out ArmyController otherArmy) && otherArmy.ownerId != 0)
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

    public void Die()
    {
        Debug.Log("Army owned by:" + ownerId + " died");
        Destroy(gameObject);
    }
}
