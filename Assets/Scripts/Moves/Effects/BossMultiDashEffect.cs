using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/Boss/MultiDash")]
public class BossMultiDashEffect : MoveEffect
{

    public override void Apply(Combatant user, Combatant victim, MoveData data)
    {
        Debug.Assert(user);
        Debug.Assert(victim);
        Debug.Assert(data);

        user.BossMultiDash();
    }
}