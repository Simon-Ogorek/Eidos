using System;
using UnityEngine;

public class HurtColliderQuereyer : MonoBehaviour
{
    string selfTag;
    public Collider collidedCombatant;
    void Start()
    {
        selfTag = transform.tag;
        collidedCombatant = null;
    }

    void OnTriggerEnter(Collider other)
    {
        // Did I hit another combatant that isnt myself?
        if (other.gameObject.GetComponent<Combatant>() && !other.transform.CompareTag(selfTag))
        {
            collidedCombatant = other;
            gameObject.SetActive(false);
        }
    }

    public void Refresh()
    {
        collidedCombatant = null;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        collidedCombatant = null;
        gameObject.SetActive(false);
    }
}
