using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudSendUnits : UiController
{
    public GameObject sendUnitPrefab;

    public GameObject holder;

    public List<ArmyData>
        castleArmy,
        sendArmy;

    public List<UnitSendPrefab>
        prefabsList;

    public Button
        buttonSend,
        buttonCancel;

    public override void OnStart()
    {
        gameObject.SetActive(false);
        sendArmy = new List<ArmyData>();
        prefabsList = new List<UnitSendPrefab>();
        buttonSend.onClick.AddListener(() => SendAmry());
        buttonCancel.onClick.AddListener(() => OnClose());
    }
    
    public override void OnOpen()
    {
        gameObject.SetActive(true);
        castleArmy = GameController.Insnatce.player.playerCastle.GetComponent<ArmyController>().armyInfo;

        if (prefabsList.Count > 0)
            prefabsList.RemoveAll(x => x);

        foreach (ArmyData army in castleArmy)
        {
            UnitSendPrefab prefab = Instantiate(sendUnitPrefab, holder.transform).GetComponent<UnitSendPrefab>();
            prefab.Initialize(army);
            prefabsList.Add(prefab);
        }
    }

    public override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void UpdateSendArmyList(ArmyData data)
    {
        if (sendArmy.Count != 0 && sendArmy.Exists(x => x.unitInfo.name == data.unitInfo.name))
        {
            sendArmy.Find(x => x.unitInfo.name == data.unitInfo.name).count = data.count;
        }
        else
            sendArmy.Add(data);
    }

    public void SendAmry()
    {
        prefabsList.ForEach(x => 
        {
            if (x.army.count > 0)
            {
                UpdateSendArmyList(x.army);
            }
        });

        if (sendArmy.Count == 0)
        {
            Debug.LogError("SendArmy is null");
            return;
        }
        sendArmy.ForEach(x => Debug.Log(x.unitInfo + " " +x.count));
        Debug.Log("Trying to send army");
    }
}
