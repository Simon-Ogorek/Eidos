using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;
using System.Numerics;

public class BattleManager : MonoBehaviour
{

    public enum BattleState
    {
        Active,
        Inactive
    }

    [SerializeField]
    private float arenaRadiusSize;

    [SerializeField]
    private float enemyAttentionRadius; 

    [SerializeField]
    private float minArenaRadius; 
    // this is such a shit name, FIX
    [SerializeField]
    private float arenaRadiusExpansionRelativeToPlayer; 

    private BattleState state;  

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private GameObject arenaVisualPrefab;
    private Material arenaVisualMat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.Inactive;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BattleState.Active)
        {
            float distanceFromCenterCoefficient = Vector3.Distance()
        }
    }

    private Vector3 DetermineCenterOfBattle(List<Transform> combatantLists)
    {
        Vector3 centerPoint = Vector3.zero;
        
        foreach (Transform combatant in combatantLists)
        {
            centerPoint += transform.position;
        }

        return centerPoint / combatantLists.Count;
    }

    public void StartBattle()
    {
        if(state == BattleState.Inactive){
        state = BattleState.Active;

        // This can be done more efficiently later and not be poo poo TODO
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        List<Transform> combatantList = new List<Transform>();
        foreach (Enemy enemy in allEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, playerMovement.transform.position) < enemyAttentionRadius)
            {
                combatantList.Add(enemy.transform);
            }
        }

        if (combatantList.Count <= 0)
        {
            Debug.LogWarning("Tried to start a battle with no enemies nearby");
            state = BattleState.Inactive;
            return;
        }

        Combatant[] allCombatants = FindObjectsByType<Combatant>(FindObjectsSortMode.None);
        foreach (Combatant combatant in allCombatants)
        {
            if (Vector3.Distance(combatant.transform.position, playerMovement.transform.position) < enemyAttentionRadius)
            {
                combatantList.Add(combatant.transform);
            }
        }

        Debug.LogWarning(combatantList.Count);

        Vector3 centerOfArena = DetermineCenterOfBattle(combatantList);

        float arenaRadius = Vector3.Distance(centerOfArena, playerMovement.transform.position);

        arenaRadius += arenaRadiusExpansionRelativeToPlayer;
        
        CapsuleCollider arenaCollider = gameObject.AddComponent<CapsuleCollider>();

        arenaCollider.isTrigger = true;
        arenaCollider.radius = arenaRadius;
        arenaCollider.center = transform.InverseTransformPoint(centerOfArena);
        arenaCollider.height = 100;

        GameObject arenaVisual = Instantiate(arenaVisualPrefab);
        arenaVisual.transform.position = centerOfArena;
        arenaVisual.transform.localScale = new Vector3(arenaRadius * 2, 1000, arenaRadius * 2);

        arenaVisualMat = arenaVisual.GetComponent<Renderer>().material;

        }
    }

    public void EndBattle()
    {
        state = BattleState.Inactive;
    }
}
