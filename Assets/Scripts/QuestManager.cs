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
    private GameObject Ship;

    [SerializeField]
    private NonCombatant Archimedes;

    [SerializeField]
    private NonCombatant Captain;

    [SerializeField]
    private NonCombatant Navigator;

    private NonCombatant playerDialogue;

    private PlayerMovement movement;

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
        movement = Player.GetComponent<PlayerMovement>();
    }

    public void StartQuest()
    {
        if (!inQuest)
        {
            inQuest = true;
            //UIController.Instance.NotificationPop("You are going to try out different activities!", "Main Quest: Discovering your Passions", true);
            //UIController.Instance.SetQuest("Discovering your Passions");
            //NonCombatant playerDialogue = Player.GetComponent<NonCombatant>();
            //ProgressDay();
            //ProgressDay();
            //ProgressDay();
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
            StartCoroutine(UIController.Instance.FadeIn());
            playerDialogue.GiveQuestDialogue("Look! I think we're almost there! (Press KEY I to Interact)", "Speech");
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
            StartCoroutine(UIController.Instance.FadeOut());  
            Ship.transform.position = new Vector3(361.76f, -3.21f, -43.35f); 
            Captain.transform.position = new Vector3(361.08f, 1.201f, -38.85f);  
            Navigator.transform.position = new Vector3(362.62f, 1.201f, -38.85f);
            SetPlayerPosition(new Vector3(361.82f, 1.186385f, -38.35f));
            ProgressDay();
            StartCoroutine(UIController.Instance.FadeIn());      
            questActionComplete = true;   
            ContinueQuest();
        }
        if(questScript == 5 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Are there other voyagers on boats starting their own quests?", "Thought");
            questActionComplete = true;
        }
        if(questScript == 6 && !questActionComplete){
            Navigator.GiveQuestDialogue("Though your paths may cross, you must undertake this quest independently. The lessons you learn must be your own.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 7 && !questActionComplete){
            Captain.GiveQuestDialogue("However, that doesn’t mean you’ll be alone. We’ve arranged for someone to accompany you. His name is Archimedes.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 8 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Wasn’t Archimedes the ancient Greek mathematician who invented ways to accomplish difficult things easily–like lifting heavy objects and pumping water from a deep well?", "Speech");
            questActionComplete = true;
        }
        if(questScript == 9 && !questActionComplete){
            Navigator.GiveQuestDialogue("Exactly. Like his Greek namesake, our Archimedes will give you tools to make your work easier and more effective. But you must make the effort yourself.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 10 && !questActionComplete){
            StartCoroutine(UIController.Instance.FadeOut());  
            Ship.SetActive(false); 
            Captain.gameObject.SetActive(false);  
            Navigator.gameObject.SetActive(false);  
            SetPlayerPosition(new Vector3(361.82f, 1.08f, 8.271f));
            ProgressDay();
            StartCoroutine(UIController.Instance.FadeIn());      
            questActionComplete = true;   
            ContinueQuest();
        }
        if(questScript == 11 && !questActionComplete){
        playerDialogue.GiveQuestDialogue("Those were some nice sailors, maybe I should look around for this Archimedes.", "Thought");
        questActionComplete = true;
        }
        if(questScript == 12 && !questActionComplete){
        playerDialogue.GiveQuestDialogue("Use WASD to walk around and the MOUSE for camera control. Press LEFT SHIFT to run.", "Thought");
        questActionComplete = true;
        }

        
    }
    public void Quest1()
    {
        if(questScript == 0 && !questActionComplete){
            Archimedes.GiveQuestDialogue("Hello, Marketus. My name is Archimedes.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 1 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Hello. The captain and navigator told me to expect you. Are you an inventor, too, like the other Archimedes?", "Speech");
            questActionComplete = true;
        }
        if(questScript == 2 && !questActionComplete){
            Archimedes.GiveQuestDialogue("Nowhere near as accomplished as he was, but I have invented this.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 3 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Archimedes gives me an item and I unwrap it, it fits in my palm.", "Thought"); 
            questActionComplete = true;
        }
        if(questScript == 4 && !questActionComplete){
            Archimedes.GiveQuestDialogue("It is a tessares makhana. In Greek, that means ‘a machine of four elements.’ But for simplicity, I just call it a Tessamark.", "Speech"); 
            questActionComplete = true;
        }
        if(questScript == 5 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("What’s it do?", "Speech"); 
            questActionComplete = true;
        }
        if(questScript == 6 && !questActionComplete){
            Archimedes.GiveQuestDialogue("At the moment, nothing. By the time we reach the other side of that mountain range, though, you will see what it can do.", "Speech"); 
            questActionComplete = true;
        }
        if(questScript == 7 && !questActionComplete){
            Archimedes.GiveQuestDialogue("In fact, learning to use the Tessamark is key to your quest’s success. For now, put it your pocket and keep it safe. Let's start going to the forest.", "Speech"); 
            questActionComplete = true;
        }
        if(questScript == 8 && !questActionComplete){
            StartCoroutine(MoveActor(Archimedes.gameObject, new Vector3(355.59f,6.21f,154.76f)));
            questActionComplete = true;
            ContinueQuest();        }
        if(questScript == 9 && !questActionComplete){
            Archimedes.AdvanceDialogueGroup("QUEST1"); 
            questActionComplete = true;
        }
        if(questScript == 10 && !questActionComplete){
            playerDialogue.GiveQuestDialogue("Will all the incoming voyagers have a guide too?", "Speech"); 
            questActionComplete = true;
        }
        if(questScript == 11 && !questActionComplete){
            Archimedes.GiveQuestDialogue("Unfortunately, no. Many will wander about fruitlessly and miss out on the knowledge this land offers. Lets continue now, the journey is long.", "Speech");
            questActionComplete = true;
        }
        if(questScript == 12 && !questActionComplete){
            StartCoroutine(MoveActor(Archimedes.gameObject, new Vector3(318.4f,8.49f,227.05f))); 
            questActionComplete = true;
        }

        
    }

    IEnumerator MoveActor(GameObject actor, Vector3 position, bool startBaseball = false)
    {
        yield return StartCoroutine(UIController.Instance.FadeOut());
        actor.transform.position = position;
        CameraController.Instance.FocusOn(actor);

        yield return new WaitForSeconds(2f);

        CameraController.Instance.FocusOn(Player);

        if(startBaseball)
            ActivityManager.Instance.StartBaseballMatch();
        
        yield return StartCoroutine(UIController.Instance.FadeIn());

    }

    public void PlayBaseball(GameObject pitcher)
    {
        if(inQuest){
        //    StartCoroutine(MoveActor(pitcher.gameObject, new Vector3(pitcher.gameObject.transform.position.x + 10, pitcher.gameObject.transform.position.y, pitcher.gameObject.transform.position.x + 10), true));
        ActivityManager.Instance.StartBaseballMatch();
    }}

    public void SetPlayerPosition(Vector3 position)
    {
         movement.enabled = false;
            Player.GetComponent<CharacterController>().enabled = false;
            Player.transform.position = position;
            Player.GetComponent<CharacterController>().enabled = true;
            movement.enabled = true;
    }
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
