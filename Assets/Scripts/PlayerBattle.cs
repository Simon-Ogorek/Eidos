using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerBattle : Combatant
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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