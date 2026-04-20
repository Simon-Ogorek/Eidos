using UnityEngine;
using System.Collections;


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
            UIController.Instance.SetQuest("Play Baseball with Archimedes");
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
            Archimedes.GiveQuestDialogue("Wow it worked!");
            questActionComplete = true;
        }
        else if(questScript == 3 && !questActionComplete){
            Archimedes.GiveQuestDialogue("I cant believe this worked but what is the limit for the amount of words that one can have in this discussion of what one may call life!");
            questActionComplete = true;
        }
        else if(questScript == 4 && !questActionComplete){
            StartCoroutine(MoveActor(Archimedes.gameObject, new Vector3(0, 1, 50)));
            questActionComplete = true;
            ContinueQuest();
        }
        if(questScript == 5 && !questActionComplete){
            Archimedes.AdvanceDialogueGroup("QUEST2");
            questActionComplete = true;
        }
        else if(questScript == 6 && !questActionComplete){
            StartCoroutine(MoveActor(Archimedes.gameObject, new Vector3(0, 1, 73), true));
            questActionComplete = true;
        }

        
    }

    IEnumerator MoveActor(GameObject actor, Vector3 position, bool startBaseball = false)
    {
        actor.transform.position = position;
        CameraController.Instance.FocusOn(actor);

        yield return new WaitForSeconds(2f);

        CameraController.Instance.FocusOn(Player);

        if(startBaseball)
            ActivityManager.Instance.StartBaseballMatch();

    }
}
