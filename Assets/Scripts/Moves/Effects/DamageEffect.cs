using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Effects/Damage")]
public class DamageEffect : MoveEffect
{

    public override void Apply(Combatant user, MoveData data)
    {
        Debug.Log("Imagine there is a cool punch visual and audio effect in the code here");
        Debug.Assert(user);
        Debug.Assert(user.target);
        Debug.Assert(data);

        user.target.ChangeHealth(data.output);
    }
}