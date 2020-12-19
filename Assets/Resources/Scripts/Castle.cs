using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private int 
        coinsCurrent,
        peoplesCurrent,
        coinsPerTurn,
        poeplesPerTurn;

    [HideInInspector]
    public int ownerId;

    [HideInInspector]
    public (int xcord, int ycord) location;

    private void Start()
    {
        coinsCurrent = GameController.Insnatce.startCoins;
        peoplesCurrent = GameController.Insnatce.startPeoples;
    }

    private void OnDestroy()
    {
        if(gameObject.GetComponent<PlayerController>() != null)
        {
            //Debug.Log("Game over. Catle message");
        }
        else
        {
            //Debug.Log("Player gets 50% of destroyed castle gold");
        }
    }
}