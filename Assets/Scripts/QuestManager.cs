using UnityEngine;


public class QuestManager : MonoBehaviour
{

    public enum CurrentMember
    {
        Player,
        Eidos
    }


    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private NonCombatant Archimedes;

    private NonCombatant playerDialogue;

    [SerializeField]
    private float day;

    [SerializeField]
    private bool inQuest = false;

    [SerializeField]
    private float quest = 0;

    [SerializeField]
    private float questScript = 0;

    [SerializeField]
    private bool questActionComplete = false;
    public static QuestManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CurrentMember selected;

     void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        playerDialogue = Player.GetComponent<NonCombatant>();
    }

    public void StartQuest()
    {
        if (!inQuest)
        {
            inQuest = true;
            UIController.Instance.SetQuest("A quest has begun");
            NonCombatant playerDialogue = Player.GetComponent<NonCombatant>();
            playerDialogue.GiveDialogue();
            questScript = 0;
            quest++;
        }
        else
            Debug.Log("Currently in quest");
    }


    // Update is called once per frame
    void Update()
    {
        if(quest == 1)
            Quest1();
        //LookTowards(Eidos);
    }

    public void ContinueQuest()
    {
        questScript += 1;
        questActionComplete = false;
        Debug.Log("continued quest" + questScript);
    }
    public void Quest1()
    {
        if(questScript == 0 && !questActionComplete){
            Archimedes.AdvanceDialogueGroup("QUEST1");
            questActionComplete = true;
        }
        else if(questScript == 1 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("This is true!");
            questActionComplete = true;
        }
        else if(questScript == 2 && !questActionComplete){
            //Archimedes.GiveQuestDialogue("Wow it worked!");
            questActionComplete = true;
        }

        
    }
}
