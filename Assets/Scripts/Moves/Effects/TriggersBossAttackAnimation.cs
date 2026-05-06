using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/Boss/TriggersAttackAnimation")]
public class TriggersBossAttackAnimation : MoveEffect
{

    public override void Apply(Combatant user, Combatant victim, MoveData data)
    {

        user.TriggerBossAttackAnimation();
    }
}