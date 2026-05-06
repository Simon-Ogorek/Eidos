using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;


public enum targetTypes
{
    Self,
    Enemy,
    Party
}



[CreateAssetMenu(fileName = "MoveData", menuName = "Combat/MoveData")]
public class MoveData : ScriptableObject
{
    public string moveName;
    
    public float castTime;

    public float output;
    public float cooldown;
    public float range;
    public float manaChange;
    public targetTypes targetType;
    public MoveEffect[] effects;

    [SerializeField]
    public BattleManager.ColliderTypes collider;
    public float colliderTTL;
    [Header("For Box Collider Only")]
    public float hurtBoxrange;
    public float hurtBoxWidth;
    [Header("For Spread Collider Only")]
    public float spreadRange;
    public float spreadWidth;
    [Header("For Projectile Only")]
    public GameObject projectileObj;
    public float projectileForce;

    public int castWeight;
    public GameObject visual;
    public float timeToWaitForVisual; // How many seconds to wait for the impact frame

}