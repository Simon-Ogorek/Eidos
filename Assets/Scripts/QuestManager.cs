using UnityEngine;
using System.Collections;


public class QuestManager : MonoBehaviour
{

    public enum CurrentMember
    {
        Player,
        Eidos
    }

    //Day System
    public enum TimeofDay
    {
        Morning,
        Noon,
        Evening,
        Night
    }

    TimeofDay currentTime;

    public Light Sun;

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private NonCombatant Archimedes;

    [SerializeField]
    private NonCombatant Captain;

    [SerializeField]
    private NonCombatant Navigator;

    private NonCombatant playerDialogue;

    [SerializeField]
    private float day = 1;

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
        currentTime = TimeofDay.Morning;
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
            UIController.Instance.NotificationPop("You are going to try out different activities!", "Main Quest: Discovering your Passions", true);
            //UIController.Instance.SetQuest("Discovering your Passions");
            NonCombatant playerDialogue = Player.GetComponent<NonCombatant>();
            ProgressDay();
            ProgressDay();
            ProgressDay();
            //playerDialogue.GiveDialogue();
            questScript = 0;
            quest++;
        }
        else
            Debug.Log("Currently in quest");
    }


    // Update is called once per frame
    void Update()
    {
        if(quest == 0)
            Quest0();
        else if(quest == 1)
            Quest1();
        //LookTowards(Eidos);
    }

    public void ContinueQuest()
    {
        questScript += 1;
        questActionComplete = false;
        Debug.Log("continued quest" + questScript);
    }

    public void Quest0()
    {
        if(questScript == 0 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Look! I think we're almost there!", "Speech");
            questActionComplete = true;
        }
        if(questScript == 1 && !questActionComplete){
            Captain.GiveQuestDialogue("It is an extraordinary view, isn't it?", "Speech");
            questActionComplete = true;
        }
        if(questScript == 2 && !questActionComplete){
            Navigator.GiveQuestDialogue("Yes, and after all these years, I still remember the mix of emotions I felt when I first arrived. Excitement, energy and a little fear", "Speech");
            questActionComplete = true;
        }
        if(questScript == 3 && !questActionComplete){
            Captain.GiveQuestDialogue("It feels good, helping this young adventurer begin his own quest.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 4 && !questActionComplete){
            UIController.Instance.FadeOut();  
            //Captain.transform.position = new Vector3(361.08, 1.201, -48.85);          
            questActionComplete = true;
        }
        
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

    public void PlayBaseball(GameObject pitcher)
    {
        if(inQuest){
        //    StartCoroutine(MoveActor(pitcher.gameObject, new Vector3(pitcher.gameObject.transform.position.x + 10, pitcher.gameObject.transform.position.y, pitcher.gameObject.transform.position.x + 10), true));
        ActivityManager.Instance.StartBaseballMatch();
    }}
    public void ProgressDay()
    {
        if (currentTime == TimeofDay.Morning)
        {
            currentTime = TimeofDay.Noon;
            Sun.intensity = 130000; 
            UIController.Instance.SetDay("Day " + day + ": Noon");
        }
        else if (currentTime == TimeofDay.Noon)
        {
            currentTime = TimeofDay.Evening;
            Sun.intensity = 70000; 
            UIController.Instance.SetDay("Day " + day + ": Evening");
        }
        else if (currentTime == TimeofDay.Evening)
        {
            currentTime = TimeofDay.Night;
            Sun.intensity = 30000; 
            UIController.Instance.SetDay("Day " + day + ": Night");
        }
        else
        {
            currentTime = TimeofDay.Morning;
            Sun.intensity = 100000; 
            day+=1;
            UIController.Instance.SetDay("Day " + day + ": Morning");
        }
    }
}
