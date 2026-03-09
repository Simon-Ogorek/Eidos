using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Deals with player interaction
/// </summary>
public class PlayerInteract : MonoBehaviour
{

    private bool touchingNPC = false;
    private GameObject lastTouchedNPC;
    void Update()
    {
        if(touchingNPC && Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Touched NPC");
            lastTouchedNPC.GetComponent<NonCombatant>().GiveDialogue();

        }
        
    }

    void OnTriggerEnter(Collider entity)
    {
        if(entity.tag == "NPC")
        {
            touchingNPC = true;
            lastTouchedNPC = entity.gameObject;
        }
    }

    void OnTriggerExit(Collider entity)
    {
        if(entity.tag == "NPC")
        {
            touchingNPC = false;
        }
    }
}
