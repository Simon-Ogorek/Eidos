using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : Combatant
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            partyManager.GetComponent<PartyManager>().SwitchMember();
        }
    }
}