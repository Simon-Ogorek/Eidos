using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using NUnit.Framework;
using UnityEngine.UIElements;

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

    public static Collider ActivateHurtCollider(Combatant caster, MoveData data)
    {
        if (data.collider == BattleManager.ColliderTypes.Box)
        {
            
            Vector3 size = new Vector3(data.hurtBoxWidth, 5, data.hurtBoxrange);

            return caster.SetHurtbox(size);            
        }
        else if (data.collider == BattleManager.ColliderTypes.Spread)
        {
            Vector3 size = new Vector3(data.spreadWidth, 5, data.spreadRange);

            return caster.SetSpread(size);  
        }
        else if (data.collider == BattleManager.ColliderTypes.Projectile)
        {
            Vector3 force = new Vector3(UnityEngine.Random.Range(-0.01f,0.01f), UnityEngine.Random.Range(-0.01f,0.01f), data.projectileForce);
            return caster.ShootProjectile(data.projectileObj, force);
        }
        return null;
    }

    static IEnumerator CastSpell(Combatant caster, MoveData data)
    {
        if (caster is PlayerBattle)
        {
            UIController.Instance.startCooldown();
            caster.GetComponent<PlayerMovement>().StartCastMovement(data.castTime);
        }
        else
        {
            caster.StartCastMovement(data.castTime);
        }

        if (data.manaChange != 0)
            caster.ChangeMana(data.manaChange);

        yield return new WaitForSeconds(data.castTime);

        Collider col = ActivateHurtCollider(caster,data);

        if (col != null)
        {
            HurtColliderQuereyer colQuereyer = col.gameObject.GetComponent<HurtColliderQuereyer>();
            colQuereyer.Refresh();
            float timeLeft = data.colliderTTL;
            while (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                if (colQuereyer.collidedCombatant != null)
                {
                    foreach (MoveEffect effect in data.effects)
                    {
                        effect.Apply(caster, colQuereyer.collidedCombatant.gameObject.GetComponent<Combatant>(), data);
                        timeLeft = -1;
                    }
                }
                yield return new WaitForNextFrameUnit();
            }
            colQuereyer.Disable();
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
