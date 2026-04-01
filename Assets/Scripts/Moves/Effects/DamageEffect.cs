using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/Damage")]
public class DamageEffect : MoveEffect
{

    public override void Apply(Combatant user, MoveData data)
    {
        Debug.Log($"Punching for {data.output} {user.name} to {user.target.name}");
        Debug.Assert(user);
        Debug.Assert(user.target);
        Debug.Assert(data);

        user.target.ChangeHealth(data.output);
    }
}