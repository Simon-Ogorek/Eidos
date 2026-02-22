using UnityEngine;

/// <summary>
/// Any given entity that can enter combat is a combatant
/// </summary>
public class Combatant : MonoBehaviour
{
    [field: SerializeField]
    public float health { get; private set; } = 5f;
    [field: SerializeField]
    public float maxHealth { get; private set; } = 5f;

    [field: SerializeField]
    public float mana { get; private set; } = 5f;
    [field: SerializeField]
    public float maxMana { get; private set; } = 5f;
    [field: SerializeField]
    public Texture2D portrait { get; private set; }
    [field: SerializeField]
    public string displayName { get; private set; }

    [SerializeField]
    protected Transform player;
    [SerializeField]
    public bool isEnemy;
    [SerializeField]
    protected GameObject partyManager;
    [SerializeField]
    protected GameObject battleManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Player is controlling the selected
    public void SwitchOn()
    {
        gameObject.GetComponent<CharacterController>().enabled = true;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<Follow>().enabled = false;

    }
    //Member is switched to a follower
     public void SwitchOff()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<Follow>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
    }

    
}