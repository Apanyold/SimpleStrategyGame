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
    public GameObject owner;

    [HideInInspector]
    public (int xcord, int ycord) location;

    private void Start()
    {
        coinsCurrent = GameController.Insnatce.startCoins;
        peoplesCurrent = GameController.Insnatce.startPeoples;
    }
}