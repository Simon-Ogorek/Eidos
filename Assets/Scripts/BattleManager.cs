using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Unity.Mathematics;
using UnityEngine.UIElements;

/// <summary>
/// This keeps tracks of if a battle is currently ongoing, activating UI and creating the arena for a given
/// battle. 
/// </summary>
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    public enum BattleState
    {
        Active,
        Inactive
    }
    public enum ColliderTypes
    {
        Box,
        Spread,
        Projectile,
        None
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
    public Vector3 centerOfArena;
    public float arenaRadius;
    List<Transform> combatantList;
    
    void Awake()
    {
        Instance = this;
    }
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
            #region ArenaUpdate

            float distanceFromCenterCoefficient = Vector3.Distance(playerMovement.transform.position, centerOfArena) /
              arenaVisualInstance.transform.localScale.x/2;

            //Debug.Log(distanceFromCenterCoefficient);
            if (distanceFromCenterCoefficient > 0.7f)
                arenaVisualMat.color = new Color(arenaVisualMat.color.r, arenaVisualMat.color.g, arenaVisualMat.color.b, distanceFromCenterCoefficient * 180/255);
            else
                arenaVisualMat.color = new Color(arenaVisualMat.color.r, arenaVisualMat.color.g, arenaVisualMat.color.b, 0);
            
            #endregion

            #region BattleUpdate

            
            


            #endregion

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
        PlayerBattle player = null;
        if(state == BattleState.Inactive){
            state = BattleState.Active;


            Combatant[] allCombatants = FindObjectsByType<Combatant>(FindObjectsSortMode.None);
            combatantList = new List<Transform>();

            foreach (Combatant combatant in allCombatants)
            {
                if (Vector3.Distance(combatant.transform.position, playerMovement.transform.position) < enemyAttentionRadius)
                {
                    combatantList.Add(combatant.transform);
                    if (combatant is Enemy enemyClass)
                    {
                        StartCoroutine(enemyClass.TryForMovement());
                        UIController.Instance.AddToEnemyPanel(combatant);
                    }
                    if (combatant is PlayerBattle playerBattle)
                    {
                        player = playerBattle;
                    }
                        
                }
            }

            UIController.Instance.SetState(UIController.UIState.Battle);

            Debug.LogWarning(combatantList.Count);

            centerOfArena = DetermineCenterOfBattle(combatantList);
            Debug.Log("Center: " + centerOfArena);
            arenaRadius = Vector3.Distance(centerOfArena, playerMovement.transform.position);

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

            player.target = UIController.Instance.ResetTarget();
            

        }
        else
        {
            Debug.Log("Trying to start a new battle when one already exists");
            return;
        }
    }

    public void EndBattle()
    {
        AudioController.Instance.BattlePlayWin();
        Destroy(arenaVisualInstance);
        state = BattleState.Inactive;
        UIController.Instance.hideBattleUI();
        UIController.Instance.SetState(UIController.UIState.Exploring);
    }

    public void RemoveFromBattle(Combatant ent)
    {
        Debug.Log($"Removing UI of a {ent.GetType()}");
        if (ent is Enemy)
        {
            Debug.Log($"Removing {ent.name} from battle");
            
            UIController.Instance.RemoveFromEnemyPanel(ent);
            combatantList.Remove(ent.transform);

            // Check if we have any enemies left in the battle;
            foreach (Transform combatant in combatantList)
            {
                Enemy enemy = combatant.gameObject.GetComponent<Enemy>();
                if (enemy != null && enemy.enabled)
                {
                    return;
                }
            }

            // There are no enemies left in the battle.
            EndBattle();
        }
    }

}
