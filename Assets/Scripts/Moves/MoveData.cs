using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;


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
    public float hurtBoxrange;
    public float hurtBoxWidth;
    public int castWeight;

}