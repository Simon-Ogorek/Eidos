using Unity.AI.Navigation;
using UnityEngine;

using UnityEngine.AI;

[CreateAssetMenu(menuName = "Combat/Effects/DillyDally")]
public class DillyDally : MoveEffect
{

    public override void Apply(Combatant user, Combatant victim, MoveData data)    {
        Debug.Log("Straight up Dilly Dallying (Random Roaming)");
        Debug.Assert(user);
        Debug.Assert(data);
        
        Vector3 randomPoint = Random.insideUnitSphere;
        var battleManager = GameObject.FindAnyObjectByType<BattleManager>();
        randomPoint *= battleManager.arenaRadius;
        randomPoint += battleManager.centerOfArena;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, float.MaxValue, NavMesh.AllAreas))
        {
            user.GetComponent<NavMeshAgent>().SetDestination(hit.position);
        }

        
    }
}