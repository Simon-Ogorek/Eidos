using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Every move usuable by a player, eidos, or monster is handled here. 
///  Must be associated with a Combatant Sub-Class
/// 
///  Combatants have associated Moves that have a function to validate casting of the spell,
///  and execution of the actual spell. every instance of a Move is aware of 
///  the combatant for the sake of being able to figure out 
///  who the target of x Combatant is, and for applying status effects.
/// </summary>
public class Move : MonoBehaviour
{
    /// @brief Who this move effects
    public enum targetTypes
    {
        Self,
        Enemy
    }

    /// <summary>
    /// Defines a move | 
    /// validator checks to see if the move is able to be done. 
    /// performer does the move. 
    /// target specifies if its for targetting enemies or for targeting yourself. 
    /// name is used for UI purposes.
    /// </summary>
    
    public struct MoveDefinition
    {
        public Action validator;
        public Action performer;
        public targetTypes target;
        public string name;

        public MoveDefinition(Action Validator, Action Performer, targetTypes Target, string Name)
        {
            validator = Validator;
            performer = Performer;
            target = Target;
            name = Name;
        }
    }

    /// @brief Who this move is assigned to, assigned automatically
    protected Combatant caster;
    private enum MoveRegistry
    {
        Attack,
        Block,
    };

    /// @brief What move is this
    [SerializeField] private MoveRegistry selectedMove;
    private Dictionary<MoveRegistry, MoveDefinition> moveFunctionLookup;
    
    void Awake()
    {
        moveFunctionLookup = new Dictionary<MoveRegistry, MoveDefinition>()
        {
            { MoveRegistry.Attack, new MoveDefinition(Check_Attack, Do_Attack, targetTypes.Enemy, "Attack") },
            { MoveRegistry.Block, new MoveDefinition(Check_Block, Do_Block, targetTypes.Self, "Block") },
        };
    }

    /// <summary>
    /// shorthand to get a move's definition
    /// </summary>
    /// <returns>MoveDefinition</returns>
    MoveDefinition GetMove()
    {
        return moveFunctionLookup[selectedMove];
    }

    void Start()
    {

        caster = GetComponentInParent<Combatant>();
        if (caster == null)
        {
            Debug.LogError(GetMove().name + " needs to be a child of a caster (Combatant sub-class)");
        }
        
    }
    public void Check_Attack()
    {
        
    }

    public void Do_Attack()
    {
        
    }

    public void Check_Block()
    {
        
    }

    public void Do_Block()
    {
        
    }

}
