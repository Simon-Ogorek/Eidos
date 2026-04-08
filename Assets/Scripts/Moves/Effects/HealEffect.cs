using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/HealSelf")]
public class HealEffect : MoveEffect
{

    public override void Apply(Combatant user, Combatant victim, MoveData data)
    {
        Debug.Log($"Healing {user} for {data.output}");
        user.ChangeHealth(data.output);
    }
}