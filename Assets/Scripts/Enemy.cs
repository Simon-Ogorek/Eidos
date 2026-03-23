using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.UI;
using UnityEngine;

public class Enemy : Combatant
{
    [SerializeField]
    private float awarenessRange = 5f;

    [SerializeField]
    private float movementOpportunityInterval = 3f;
    [SerializeField]

    private float movementOpportunityChance = 0.4f;

    [Serializable]
    public struct AIAction
    {
        public int weight; // How likely to happen
        public MoveData move; // What to execute

    }

    [SerializeField]
    public List<AIAction> listOfActions = new List<AIAction>();

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = FindAnyObjectByType<PlayerBattle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < awarenessRange)
        {
            battleManager.GetComponent<BattleManager>().StartBattle();
        }
    }

    void ExecuteMove(MoveData move)
    {
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

        remainingCooldown = move.cooldown;
        StartCoroutine(TryForMovement());
    }

    public IEnumerator TryForMovement()
    {
        yield return new WaitForSeconds(remainingCooldown);
        yield return new WaitForSeconds(movementOpportunityInterval);

        if (UnityEngine.Random.Range(0,1) < movementOpportunityChance)
        {
            Debug.Log("Movement opportunity passed, doing some action");

            int i = 0;
            int totalWeights = 0;

            for (; i < listOfActions.Count; i++)
            {
                totalWeights += listOfActions[i].weight;
            }

            int weightToFind = UnityEngine.Random.Range(0, totalWeights+1);

            for (i = 0; i < listOfActions.Count && weightToFind > listOfActions[i].weight; i++)
            {
                weightToFind -= listOfActions[i].weight;
            }

            ExecuteMove(listOfActions[i].move);

        }
        else
        {
            Debug.Log("Movement opportunity didnt succeed for enemy, Idling");
        }


    }

    public void UpdateBattle()
    {
        
    }
}
