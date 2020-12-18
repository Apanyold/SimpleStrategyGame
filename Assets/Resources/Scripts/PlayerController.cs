using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Castle playerCastle;

    void Start()
    {
        playerCastle = GetComponent<Castle>();
        playerCastle.owner = gameObject;
    }
}
