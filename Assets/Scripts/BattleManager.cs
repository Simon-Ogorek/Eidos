using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;

/// <summary>
/// This keeps tracks of if a battle is currently ongoing, activating UI and creating the arena for a given
/// battle. 
/// </summary>
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
    private GameObject arenaVisualInstance;
    private Material arenaVisualMat;
    private Vector3 centerOfArena;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = BattleState.Inactive;
    }

    /// @brief Fade the arena visual in/out depending on player distance
    void Update()
    {
        if (state == BattleState.Active && arenaVisualInstance)
        {
            float distanceFromCenterCoefficient = Vector3.Distance(playerMovement.transform.position, centerOfArena) /
              arenaVisualInstance.transform.localScale.x/2;

            Debug.Log(distanceFromCenterCoefficient);
            if (distanceFromCenterCoefficient > 0.7f)
                arenaVisualMat.color = new Color(arenaVisualMat.color.r, arenaVisualMat.color.g, arenaVisualMat.color.b, distanceFromCenterCoefficient * 180/255);
            else
                arenaVisualMat.color = new Color(arenaVisualMat.color.r, arenaVisualMat.color.g, arenaVisualMat.color.b, 0);

        }
    }

    /// @brief used for determining where the center is in accordance to all combatants
    private Vector3 DetermineCenterOfBattle(List<Transform> combatantLists)
    {
        Vector3 centerPoint = Vector3.zero;
        
        foreach (Transform combatant in combatantLists)
        {
            centerPoint += combatant.transform.position;
            Debug.Log("Transform is" + centerPoint);
        }

        Debug.Log("Transform returned is" + centerPoint);
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

        //Reset combatant list so enemies aren't counted twice
        combatantList = new List<Transform>();

        Combatant[] allCombatants = FindObjectsByType<Combatant>(FindObjectsSortMode.None);
        foreach (Combatant combatant in allCombatants)
        {
            if (Vector3.Distance(combatant.transform.position, playerMovement.transform.position) < enemyAttentionRadius)
            {
                combatantList.Add(combatant.transform);
                if (combatant.isEnemy)
                    UIController.Instance.AddToEnemyPanel(combatant);
            }
        }
        UIController.Instance.SetState(UIController.UIState.Battle);
        

        Debug.LogWarning(combatantList.Count);

        centerOfArena = DetermineCenterOfBattle(combatantList);
        Debug.Log("Center: " + centerOfArena);
        float arenaRadius = Vector3.Distance(centerOfArena, playerMovement.transform.position);

        arenaRadius += arenaRadiusExpansionRelativeToPlayer;
        
        CapsuleCollider arenaCollider = gameObject.AddComponent<CapsuleCollider>();

        arenaCollider.isTrigger = true;
        arenaCollider.radius = arenaRadius;
        arenaCollider.center = centerOfArena;
        arenaCollider.height = 100;

        arenaVisualInstance = Instantiate(arenaVisualPrefab);
        arenaVisualInstance.transform.position = centerOfArena;
        arenaVisualInstance.transform.localScale = new Vector3(arenaRadius * 2, 1000, arenaRadius * 2);

        arenaVisualMat = arenaVisualInstance.GetComponentInChildren<Renderer>().material;
        

        }
        else
        {
            Debug.Log("Trying to start a new battle when one already exists");
            return;
        }
    }

    public void EndBattle()
    {
        state = BattleState.Inactive;
    }
}
