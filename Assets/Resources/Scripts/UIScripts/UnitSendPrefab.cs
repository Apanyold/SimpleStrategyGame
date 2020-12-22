using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSendPrefab : MonoBehaviour
{
    public Text 
        unitName,
        unitCurrentCount;
    public InputField unitCount;

    public UnitInfo unit;

    public int armyToSendCount = 0;

    public void Initialize(ArmyData army)
    {
        unit = army.unitInfo;
        unitName.text = unit.name + " :";
        unitCurrentCount.text = army.count.ToString();
        unitCount.onValueChanged.AddListener((value) =>{UpdateArmyInfo(value);});
    }

    private void UpdateArmyInfo(string value)
    {
        armyToSendCount = int.Parse(value);
        //int.TryParse(value, out int x);

        //if (x <= 0)
        //{
        //    Debug.LogError("IncorrectValue");
        //    army.count = 0;
        //}
        //else
        //    army.count = x;

    }
}
