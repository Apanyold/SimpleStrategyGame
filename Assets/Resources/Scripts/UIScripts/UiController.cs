using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour, IUi
{
    
    [SerializeField]
    public List<UiController> listUiElements;

    public string Name;

    public Button 
        sendArmy,
        endTurn;

    public void InitInterface()
    {
        Debug.Log(listUiElements.Count);

        StartCoroutine(InitHud());

        sendArmy.onClick.AddListener(() => { Open("hud_send_units"); ; });
        endTurn.onClick.AddListener(() => { TurnEnd(); });
    }

    IEnumerator InitHud()
    {
        yield return new WaitForEndOfFrame();


        listUiElements.ForEach(x => x.OnStart());
        Open("hud_buildings");
    }

    public void Open(string name)
    {
        listUiElements.Find(x => x.Name == name).OnOpen();
    }

    public void ShowNotification(string notificationText)
    {
        Debug.Log("ShowNotification with text: " + notificationText);
    }

    public void TurnEnd()
    {

    }

    public virtual void OnStart()
    {

    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {
    }
}
