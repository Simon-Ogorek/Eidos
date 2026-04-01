using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using NUnit.Framework;

/// <summary>
///  Every move usuable by a player, eidos, or monster is handled here. 
///  Must be associated with a Combatant Sub-Class
/// 
///  Combatants have associated Moves that have a function to validate casting of the spell,
///  and execution of the actual spell. every instance of a Move is aware of 
///  the combatant for the sake of being able to figure out 
///  who the target of x Combatant is, and for applying status effects.
/// </summary>
public static class MoveCaster : object
{

    /// <summary>
    /// shorthand to get a move's data
    /// </summary>

    public static void CreateHurtbox(MoveData data)
    {
        if (data.collider == BattleManager.ColliderTypes.Box)
        {
            BoxCollider col;
            
        }
        
    }

    static IEnumerator CastSpell(Combatant caster, MoveData data)
    {
        if (caster is PlayerBattle)
        {
            UIController.Instance.startCooldown();
        }

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

        if (caster is PlayerBattle)
        {
            UIController.Instance.endCooldown();
        }

        Debug.Log("Finished Cast");

        yield break;
        
    }

    public static IEnumerator DoMove(Combatant caster, MoveData data)
    {
        Debug.Log("Starting Cast");
        caster.StartCastMovement(data.castTime);
        return CastSpell(caster,data);
    }

}
