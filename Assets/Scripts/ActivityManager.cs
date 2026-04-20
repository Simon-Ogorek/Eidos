using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class ActivityManager : MonoBehaviour
{

    public enum CurrentMember
    {
        Player,
        Eidos
    }


    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameObject Baseball;

    [SerializeField]
    private NonCombatant Archimedes;

    private NonCombatant playerDialogue;

    [SerializeField]
    private float day;

    [SerializeField]
    private bool inPlay = false;

    [SerializeField]
    private float quest = 0;

    [SerializeField]
    private float questScript = 0;

    [SerializeField]
    private bool questActionComplete = false;

    bool usingController = false;
    public static ActivityManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CurrentMember selected;

     void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
      
    }



    // Update is called once per frame
    void Update()
    {

        if(Gamepad.current!=null){
            usingController = true;
        }
        else if(Gamepad.current==null)
        {
            usingController = false;
        }
        if((inPlay && Input.GetKeyDown(KeyCode.I)) || (inPlay &&  usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            Debug.Log("BASEBALL HIT");

        }
        else if ((!inPlay && Input.GetKeyDown(KeyCode.I)) || (!inPlay &&  usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            Debug.Log("BASEBALL MISS");   
        }
       // if(quest == 1)
       //     Quest1();
        //LookTowards(Eidos);
    }
/*
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
            StartCoroutine(MoveActor(Archimedes.gameObject, new Vector3(-10, 1, 30)));
            questActionComplete = true;
        }

        
    }
*/
    IEnumerator MoveActor(GameObject actor, Vector3 position)
    {
        actor.transform.position = position;
        CameraController.Instance.FocusOn(actor);

        yield return new WaitForSeconds(2f);

        CameraController.Instance.FocusOn(Player);
    }
    

    //For baseball minigame
        void OnTriggerEnter(Collider entity)
    {
        if(entity.tag == "BASEBALL")
        {
            inPlay = true;
            Debug.Log("NPC Triggered");
        }

    }

    void OnTriggerExit(Collider entity)
    {
        if(entity.tag == "BASEBALL")
        {
            inPlay = false;
        }
    }
}
