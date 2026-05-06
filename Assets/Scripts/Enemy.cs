using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Combatant
{
    [SerializeField]
    private float awarenessRange = 5f;

    // For telling when a enemy should forget that they have already triggered a battle with the player
    [SerializeField]
    private float forgetRange = 40f;


    [SerializeField]
    private float movementOpportunityInterval = 3f;
    [SerializeField]

    private float movementOpportunityChance = 0.4f;
    Coroutine currentMove;
    public bool hasTriggeredBattle = false;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindAnyObjectByType<PlayerBattle>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < awarenessRange && !hasTriggeredBattle)
        {
            
            battleManager.GetComponent<BattleManager>().StartBattle();
            hasTriggeredBattle = true;
        }
        if (dist > forgetRange)
        {
            hasTriggeredBattle = false;
            health = maxHealth;
        }

        if (isBoss)
        {
            bossAnimationController bossAnimation = GetComponentInChildren<bossAnimationController>();

            if (agent.velocity.magnitude > 0.1f)
            {
                bossAnimation.SetIsWalking(true);
            }
            else
            {
                bossAnimation.SetIsWalking(false);
            }
        }
    }

    void ExecuteMove(MoveData move)
    {
        /*
        Debug.Log($"Enemy {name} is executing {move.name}");

        foreach (MoveEffect effect in move.effects)
        {
            if (move.manaChange >= 0 || Math.Abs(move.manaChange) <= this.mana)
            {
                effect.Apply(this as Combatant, move);
                if (move.manaChange != 0)
                    this.ChangeMana(move.manaChange);
            }
        }
        */

        currentMove = StartCoroutine(MoveCaster.DoMove(this,move));
        remainingCooldown = move.cooldown;
        StartCoroutine(TryForMovement());

    }

    public IEnumerator TryForMovement()
    {
        yield return new WaitForSeconds(remainingCooldown);
        yield return new WaitForSeconds(movementOpportunityInterval);

        currentMove = null;

        if (UnityEngine.Random.value < movementOpportunityChance)
        {
            

            int i = 0;
            int totalWeights = 0;

            for (; i < moves.Count; i++)
            {
                totalWeights += moves[i].castWeight;
            }

            int weightToFind = UnityEngine.Random.Range(0, totalWeights+1);

            for (i = 0; i < moves.Count && weightToFind > moves[i].castWeight; i++)
            {
                weightToFind -= moves[i].castWeight;
            }

            Debug.Log($"Movement opportunity passed, doing {moves[i].name} ");

            ExecuteMove(moves[i]);

        }
        else
        {
            Debug.Log("Movement opportunity didnt succeed for enemy, Idling");
            StartCoroutine(TryForMovement());
        }


    }



    public void UpdateBattle()
    {
        
    }
}
