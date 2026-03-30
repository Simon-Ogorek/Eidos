using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

/// <summary>
///  Every move usuable by a player, eidos, or monster is handled here. 
///  Must be associated with a Combatant Sub-Class
/// 
///  Combatants have associated Moves that have a function to validate casting of the spell,
///  and execution of the actual spell. every instance of a Move is aware of 
///  the combatant for the sake of being able to figure out 
///  who the target of x Combatant is, and for applying status effects.
/// </summary>
public class Move : MonoBehaviour
{
    /// @brief Who this move is assigned to, assigned automatically
    protected Combatant caster;
    public MoveData data;

    void Start()
    {
        caster = GetComponent<Combatant>();
    }
    /// <summary>
    /// shorthand to get a move's data
    /// </summary>
    public MoveData GetData()
    {
        return data;
    }

    public void CreateHurtbox()
    {
        if (data.collider == BattleManager.ColliderTypes.Box)
        {
            BoxCollider col;
            
        }
        
    }

    IEnumerator CastSpell()
    {
        UIController.Instance.endCooldown();
        UIController.Instance.startCooldown();

        yield return new WaitForSeconds(data.castTime);
        foreach (MoveEffect effect in data.effects)
        {
            if (data.manaChange >= 0 || Math.Abs(data.manaChange) <= caster.mana)
            {
                effect.Apply(caster, data);
                if (data.manaChange != 0)
                    caster.ChangeMana(data.manaChange);
            }
        }

        UIController.Instance.endCooldown();

        Debug.Log("Finished Cast");

        yield break;
        
    }

    public Coroutine DoMove()
    {
        PlayerMovement Player = caster.GetComponent<PlayerMovement>();
        if (Player)
        {
            Debug.Log("Starting Cast");
            Player.StartCastMovement(data.castTime);
            return StartCoroutine(CastSpell());
        }
        return null;

        
    }

}
