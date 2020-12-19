using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        armyInfo = new List<ArmyData>();
        unitInfoList = GameController.Insnatce.unitsInfo;
        if (TryGetComponent(out Castle C))
        {
            UpdateCatleArmyInfo();
            ifOnCastle = true;
            isMovedThisTurn = true;
            ownerId = C.ownerId;
        }

        //testing field;
        UpdateArmyInfo(new ArmyData(unitInfoList[0], 1, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[1], 2, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[2], 3, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[3], 1, 1));
        
        //GameController.Insnatce.fightController.InitiateFight(armyInfo, enemyArmyInfoTest);

        //CreateAttackOrder(enemyArmyInfoTest);

        mainGrid = GameController.Insnatce.grid;

        if(!ifOnCastle)
            isMovedThisTurn = false;
    }

    public void UpdateArmyInfo(ArmyData armyData)
    {
        if (armyInfo.Exists(x => x.unitInfo.name == armyData.unitInfo.name))
        {
            armyInfo.Find(x => x.unitInfo.name == armyData.unitInfo.name).count += armyData.count;
        }
        else
            armyInfo.Add(armyData);

        armyInfo.ForEach(x => Debug.Log("Unity name: " + x.unitInfo.name + " count: " + x.count));

        CalculateArmySpeed();
    }

    public void CalculateArmySpeed()
    {
        int min = 100;
        armyInfo.ForEach(x => {
            if (x.unitInfo.speed < min)
                min = x.unitInfo.speed;
        });
        amrySpeed = min;
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
