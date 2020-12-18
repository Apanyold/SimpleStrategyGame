using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    private static UiController instance;

    public static UiController Instance { get => instance; set => instance = value; }

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowNotification(string notificationText)
    {
        Debug.Log("ShowNotification with text: " + notificationText);
    }
}
