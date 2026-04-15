using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/Damage")]
public class DamageEffect : MoveEffect
{

    public override void Apply(Combatant user, Combatant victim, MoveData data)
    {
        Debug.Log($"Damaging for {data.output} {user.name} to {victim.name}");
        Debug.Assert(user);
        Debug.Assert(victim);
        Debug.Assert(data);

        victim.ChangeHealth(data.output);
    }
}