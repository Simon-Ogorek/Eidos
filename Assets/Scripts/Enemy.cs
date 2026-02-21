using UnityEngine;

public class Enemy : Combatant
{
    [SerializeField]
    private float awarenessRange = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < awarenessRange)
        {
            battleManager.GetComponent<BattleManager>().StartBattle();
        }
    }
}
