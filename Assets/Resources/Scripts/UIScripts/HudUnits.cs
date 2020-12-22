using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUnits : Hud
{
    public Button buttonSwitch;

    private List<UnitInfo> units;

    List<ArmyData> avaliebleToTrainUnit;
    Castle castle;

    public GameObject 
        buyPrefab,
        holder;

    public override void OnStart()
    {
        buttonSwitch.onClick.AddListener(() => { UiController.Instance.Open("hud_buildings"); OnClose(); });

    }

    public override void OnOpen()
    {
        castle = GameController.Insnatce.player.playerCastle;
        avaliebleToTrainUnit = castle.avaliebleToTrainUnits;

        gameObject.SetActive(true);
        units = GameController.Insnatce.unitsInfo;

        foreach (ArmyData uniy in avaliebleToTrainUnit)
        {

        }

        UnitBuyPrefab temp;
        foreach (ArmyData uniy in avaliebleToTrainUnit)
        {
            temp = Instantiate(buyPrefab, holder.transform).GetComponent<UnitBuyPrefab>();
            temp.Init(this, uniy.unitInfo);
        }
    }

    public override void OnClose()
    {
        gameObject.SetActive(false);
    }
    public void AttemptToBuy(UnitInfo info, int count)
    {
        if(count > 0)
        {
            
        }
    }
}
