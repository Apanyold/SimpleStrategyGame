using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public int amrySpeed;

    [HideInInspector]
    public Dictionary<UnitInfo, int> amryInfo;

    private List<UnitInfo> unitInfoList;

    private Grid mainGrid;

    public bool isMovedThisTurn;
    
    private void Start()
    {
        amryInfo = new Dictionary<UnitInfo, int>();
        unitInfoList = GameController.Insnatce.unitsInfo;

        amryInfo.Add(unitInfoList[0], 2);

        UpdateArmyInfo(unitInfoList[0], 2);

        mainGrid = GameController.Insnatce.grid;

        isMovedThisTurn = false;
    }

    public void UpdateArmyInfo(UnitInfo key, int value)
    {
        if (!amryInfo.TryGetValue(key, out int x))
        {
            amryInfo.Add(key, value);
        }
        else
        {
            amryInfo[key] = x + value;
        }

        Debug.Log("Unit: " + key.name + " Count: " + amryInfo[key]);

        CalculateArmySpeed();
    }

    public void CalculateArmySpeed()
    {
        int min = 100;
        foreach(UnitInfo unit in amryInfo.Keys)
        {
            if (unit.speed < min)
                min = unit.speed;
        }
        amrySpeed = min;
    }

    public void OnTurnEnd()
    {
        isMovedThisTurn = false;
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
        else if ((pointObject.GetComponent<ArmyController>() != null && pointObject.GetComponent<ArmyController>().owner != null))
        {
            if (pointObject.GetComponent<ArmyController>().owner == owner)
            {
                Debug.Log("Move to own Army");
            }
            else if (pointObject.GetComponent<ArmyController>().owner != owner)
            {
                Debug.Log("Move to enemy Army");
            }
        }

        isMovedThisTurn = true;

        mainGrid.MoveTo(position, gameObject);
    }

    public void StartFight(GameObject attackSubject)
    {
        while(attackSubject != null && amryInfo.Count > 0)
        {

        }
    }

    public void ArmyDie()
    {
        Destroy(gameObject);
    }
}
