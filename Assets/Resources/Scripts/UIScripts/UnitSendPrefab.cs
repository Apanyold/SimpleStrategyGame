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

    public ArmyData army;

    public void Initialize(ArmyData army)
    {
        this.army = army;
        unitName.text = army.unitInfo.name + " :";
        unitCurrentCount.text = army.count.ToString();

        unitCount.onValueChanged.AddListener((value) =>{UpdateArmyInfo(value);});
    }

    private void UpdateArmyInfo(string value)
    {
        int.TryParse(value, out int x);

        if (x > army.count)
            unitCount.text = army.count.ToString();
        if (x <= 0)
        {
            Debug.LogError("IncorrectValue");
            army.count = 0;
        }
        else
            army.count = x;

    }
}
