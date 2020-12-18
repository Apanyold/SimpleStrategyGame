using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [HideInInspector]
    public GameObject owner;

    [HideInInspector]
    public int amrySpeed = 2;

    public void MoveZone()
    {
        //GameController.Insnatce.grid

    }

    private void Start()
    {
        amrySpeed = 2;
    }
}
