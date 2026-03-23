using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Any given entity that can enter combat is a combatant
/// </summary>
public class Combatant : MonoBehaviour
{
    [field: SerializeField]
    public float health { get; protected set; } = 5f;
    [field: SerializeField]
    public float maxHealth { get; private set; } = 5f;

    [field: SerializeField]
    public float mana { get; private set; } = 5f;
    [field: SerializeField]
    public float maxMana { get; private set; } = 5f;
    [field: SerializeField]
    public float level { get; private set; } = 5f;
    [field: SerializeField]
    public float experience { get; private set; } = 5f;
    
    [field: SerializeField]
    public string displayName { get; private set; }

    [field: SerializeField]
    public Texture2D portrait { get; private set; }


    [SerializeField]
    protected Transform player;
    [SerializeField]
    public bool isEnemy;
    [SerializeField]
    protected GameObject partyManager;
    [SerializeField]
    protected GameObject battleManager;
    public Combatant target;
    public bool uiOutOfSync = false;
    public bool canDoActions { get; private set; } = false;
    public bool doingAction { get; private set; } = false;
    public bool canCancelAction { get; private set; } = false;
    protected float remainingCooldown;

    //Player is controlling the selected
    public void SwitchOn()
    {
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<Follow>().enabled = false;

    }
    //Member is switched to a follower
     public void SwitchOff()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<Follow>().enabled = true;

    }

    public void Die()
    {
        
    }

    public void ChangeHealth(float value)
    {
        health = Math.Clamp(health + value, 0, maxHealth);
        uiOutOfSync = true;
    }
    public void ChangeMana(float value)
    {
        mana = Math.Clamp(mana + value, 0, maxMana);
        uiOutOfSync = true;
    }
}