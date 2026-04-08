using UnityEngine;

public abstract class MoveEffect : ScriptableObject
{
    public abstract void Apply(Combatant user, Combatant victim, MoveData data);
}


