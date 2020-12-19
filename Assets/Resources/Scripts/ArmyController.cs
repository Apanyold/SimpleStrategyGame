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
        enemyArmyInfoTest = new List<ArmyData>();
        unitInfoList = GameController.Insnatce.unitsInfo;
        if (GetComponent<Castle>() != null)
        {
            UpdateCatleArmyInfo();
            ifOnCastle = true;
            isMovedThisTurn = true;
        }

        //testing field;
        UpdateArmyInfo(new ArmyData(unitInfoList[0], 1, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[1], 2, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[2], 3, 1));
        UpdateArmyInfo(new ArmyData(unitInfoList[3], 1, 1));

        enemyArmyInfoTest.Add(new ArmyData(unitInfoList[0], 1, 2));
        enemyArmyInfoTest.Add(new ArmyData(unitInfoList[1], 1, 2));
        enemyArmyInfoTest.Add(new ArmyData(unitInfoList[2], 1, 2));


        GameController.Insnatce.fightController.InitiateFight(armyInfo, enemyArmyInfoTest);

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

        if(pointObject == null)
        {
            Debug.Log("Move to empty point");
        }
        else if ((pointObject.GetComponent<ArmyController>() != null && pointObject.GetComponent<ArmyController>().ownerId != 0))
        {
            if (pointObject.GetComponent<ArmyController>().ownerId == ownerId)
            {
                Debug.Log("Move to own Army");
            }
            else if (pointObject.GetComponent<ArmyController>().ownerId != ownerId)
            {
                Debug.Log("Move to enemy Army");
            }
        }

        isMovedThisTurn = true;

        mainGrid.MoveTo(position, gameObject);
    }

    public void StartFight(GameObject attackSubject)
    {
        ArmyController enemyArmy;
        if (attackSubject != null && attackSubject.GetComponent<ArmyController>() != null)
            enemyArmy = attackSubject.GetComponent<ArmyController>();
        else
        {
            Debug.LogError("attackSubject is null or attackSubject.ArmyController is null");
            return;
        }

        if (armyInfo.Count > 0 && enemyArmy.armyInfo.Count > 0)
        {

        }
    }

    //private void CreateAttackOrder(List<ArmyData> enemyArmyInfo)
    //{
    //    List<ArmyData> 
    //        temp = new List<ArmyData>(),
    //        orderList = new List<ArmyData>();
    //    int max = 0;

    //    armyInfo.ForEach(x => {
    //        if (x.unitInfo.speed > max)
    //            max = x.unitInfo.speed;
    //    });

    //    temp = armyInfo.FindAll(x => x.unitInfo.speed == max);
        
    //    orderList.Add(temp[Random.Range(0, temp.Count + 1)]);

    //    for (int i = 0; i < enemyArmyInfo.Count + armyInfo.Count; i++)
    //    {

    //    }
    //}

    public void ArmyDie()
    {
        Destroy(gameObject);
    }
}
