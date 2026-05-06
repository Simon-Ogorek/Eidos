using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.AI;


/// <summary>
/// Any given entity that can enter combat is a combatant
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
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
    public float speed { get; private set; } = 0.05f;

     public float selfRealm { get; private set; } = 1f;
    [field: SerializeField]
    public float communityRealm { get; private set; } = 1f;

     public float workRealm { get; private set; } = 1f;
    [field: SerializeField]
    public float familyRealm { get; private set; } = 1f;

    float defaultSpeed;
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

    Coroutine castingCoroutine;
    bool coroutineRunning = false;
    public List<MoveData> moves;

    public GameObject CombatColliders;

    bool AgentInControl = false;
    public NavMeshAgent agent;

    public enum DeathType
    {
        Run_Away,
        Run_Off_Arena,
        Dissapear,
        Player_Death,
        Become_NPC
    }

    public DeathType deathType = DeathType.Dissapear; // TODO

    void Start()
    {
        defaultSpeed = speed;
        agent = GetComponent<NavMeshAgent>();
        //agentSpeed = agent.speed;
        agent.enabled = false;
    }

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

    IEnumerator CastMovement(float time)
    {
        speed = defaultSpeed * 0.1f;
        Debug.Log($"Cast Movement started {speed} for {name}");
        yield return new WaitForSeconds(time);
        speed = defaultSpeed;
        Debug.Log($"Cast Movement ended {speed} for {name}");


    }
    public void StartCastMovement(float time)
    {
        if (coroutineRunning)
        {
            EndCastMovement();
        }
        castingCoroutine = StartCoroutine(CastMovement(time));
    }
    public void EndCastMovement()
    {
        if (coroutineRunning)
        {
            StopCoroutine(castingCoroutine);
        }
    }

    public void Die()
    {
        BattleManager.Instance.RemoveFromBattle(this);
        Debug.Log($"{name} has died of death type ${deathType}");
        switch (deathType)
        {
            case DeathType.Run_Away:
                // TODO
                break;
            case DeathType.Run_Off_Arena:
                // TODO
                break;
            case DeathType.Dissapear:
                // Stupid Solution, Good Results
                transform.position = new Vector3(0, -100, 0);
                agent.enabled = false;
                Destroy(gameObject, 10f);
                break;
            case DeathType.Player_Death:
                // TODO
                break;
            case DeathType.Become_NPC:
                agent.enabled = false;
                becomeNPC();
                break;
        }
    }
    

    public void ChangeHealth(float value)
    {
        health = Math.Clamp(health + value, 0, maxHealth);
        Debug.Log($"New Health {health}");
        uiOutOfSync = true;

        if (health <= 0)
        {
            Die();
        }
    }
    public void ChangeMana(float value)
    {
        mana = Math.Clamp(mana + value, 0, maxMana);
        uiOutOfSync = true;
    }

    public void DisableColliders()
    {
        foreach (Transform obj in CombatColliders.transform)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public Collider SetHurtbox(Vector3 size)
    {
        Debug.Assert(CombatColliders);
        BoxCollider col = CombatColliders.GetComponentInChildren<BoxCollider>(true);
        
        col.gameObject.transform.localScale = size;
        col.gameObject.transform.localPosition = new Vector3(0,0,col.gameObject.transform.localScale.z/2);

        return col;
    }

    public Collider SetSpread(Vector3 size)
    {
        MeshCollider col = CombatColliders.GetComponentInChildren<MeshCollider>(true);
        
        col.gameObject.transform.localScale = size;
        
        return col;
    }

    public Collider ShootProjectile(GameObject obj, Vector3 force)
    {
        GameObject proj = Instantiate(obj);
        proj.transform.position = CombatColliders.transform.Find("ProjectileSpawnPoint").transform.position;
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (!proj.GetComponent<HurtColliderQuereyer>())
        {
            HurtColliderQuereyer quereyer = proj.AddComponent<HurtColliderQuereyer>();
            quereyer.tag = tag;
        }
        rb.AddForce(force);
        Debug.Log("Shot a projectile");
        return proj.GetComponent<Collider>();
    }

    public void HandoffControlToAgent()
    {
        if (agent)
            agent = GetComponent<NavMeshAgent>();

        //agent.speed = agentSpeed;
        if (this is PlayerBattle)
        {
            AgentInControl = true;
            agent.enabled = true;
            GetComponent<PlayerMovement>().canMove = false;
        }
    }

    public void TakeBackControlFromAgent()
    {
        if (agent)
            agent = GetComponent<NavMeshAgent>();
        
        
        if (this is PlayerBattle)
        {
            AgentInControl = false;
            agent.enabled = false;
            GetComponent<PlayerMovement>().canMove = true;
        }
    }

    public void SetAgentDest(Vector3 pos)
    {
        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
                agent.Warp(hit.position);
        }
        agent.SetDestination(pos);
    }

    // Not the same as taking back control, 
    // agent still maintains control but it should stop tracking.
    public void agentDisable()
    {
        //agent.speed = 0;
        agent.enabled = false;
    }

    public void agentReset()
    {
        //agent.speed = agentSpeed;
        agent.enabled = true;
    }

    public void becomeNPC()
    {
        gameObject.tag = "NPC";
        isEnemy = false;
        target = null;

        agent.enabled = false;
        DisableColliders();

        Enemy enemy = GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.enabled = false;
            enemy.isEnemy = false;
        }
        BattleManager.Instance.RemoveFromBattle(this);

        NonCombatant NPC = gameObject.GetComponent<NonCombatant>();

        if(NPC != null){
        NPC.enabled = true;
        }
        
        this.enabled = false;
    }
    public void CreateVisual(GameObject prefab)
    {
        StartCoroutine(CreateVisualHelper(prefab));
    }
    public IEnumerator CreateVisualHelper(GameObject prefab)
    {
        GameObject visual = Instantiate(prefab, transform);
        visual.transform.localPosition = new Vector3(0,0,1);
        yield return null;
        Animator animator = visual.GetComponentInChildren<Animator>();
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        while (state.normalizedTime < 1f)
        {
            state = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForNextFrameUnit();
        }

        Destroy(visual);
    } 
}