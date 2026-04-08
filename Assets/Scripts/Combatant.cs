using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;


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
    public float speed { get; private set; } = 0.05f;

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

    void Start()
    {
        defaultSpeed = speed;
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
        
    }

    public void ChangeHealth(float value)
    {
        health = Math.Clamp(health + value, 0, maxHealth);
        Debug.Log($"New Health {health}");
        uiOutOfSync = true;
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
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(force);

        return rb.gameObject.GetComponent<Collider>();
    }
}