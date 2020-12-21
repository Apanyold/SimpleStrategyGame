using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudBuildings : UiController
{
    [SerializeField]
    private GameObject
        buildingPrefab,
        buildingPrefabHolder;

    private List<BuildingPrefabContorller> buttonsList;

    Buildings buildings;
    
    public void CreateHudElements()
    {
        BuildingPrefabContorller temp;
        foreach(BuildingsData building in GameController.Insnatce.player.playerCastle.buildingsInfo)
        {
            temp = Instantiate(buildingPrefab, buildingPrefabHolder.transform).GetComponent<BuildingPrefabContorller>();
            temp.InitPrefab(building);
            buttonsList.Add(temp);
        }
    }

    public void UpdateHud()
    {
        buttonsList.ForEach(x => x.UpdatePrefabInfo());
    }

    public void ButtonClick(BuildingsData building)
    {
        Debug.Log("Button with name: "+ building.building.name);

        buildings.ActionWithBuilding(building);

        UpdateHud();
    }

    public override void OnOpen()
    {
        gameObject.SetActive(true);

        buttonsList = new List<BuildingPrefabContorller>();

        buildings = new Buildings(GameController.Insnatce.player.playerCastle);
        Debug.Log(GameController.Insnatce.player.playerCastle.name);

        CreateHudElements();
    }

    public override void OnClose()
    {
        gameObject.SetActive(true);
    }

    public override void OnStart()
    {
        gameObject.SetActive(false);
    }
}
