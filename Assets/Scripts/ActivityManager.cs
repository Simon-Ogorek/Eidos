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
    private GameObject Pitcher;

    [SerializeField]
    private GameObject Baseball;

    [SerializeField]
    private GameObject Bat;



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

    private bool inBaseball = false;

    private bool throwing = false;

    private Transform hitLocation;

    bool usingController = false;

    private float hits = 0;

    private float strikes = 0;
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
        if(inBaseball){

            if (throwing)
            {
                if(Baseball.transform.position.y < Pitcher.transform.position.y + 2.5)
                {

                    Baseball.transform.position += new Vector3(0, 0.02f, 0);
                }
                else
                {
                    throwing = false;
                    Baseball.GetComponent<BaseballPitch>().enabled = true;
                }
            }

        if(Gamepad.current!=null){
            usingController = true;
        }
        else if(Gamepad.current==null)
        {
            usingController = false;
        }
        if((inPlay && Input.GetKeyDown(KeyCode.I)) || (inPlay &&  usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            AudioController.Instance.BaseballHit();
            Debug.Log("BASEBALL HIT");
            HitBaseball();
            hits+=1;
            UIController.Instance.SetHit(hits);
            if(hits == 3)
                finishBaseball();

        }
        else if ((!inPlay && Input.GetKeyDown(KeyCode.I)) || (!inPlay &&  usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            AudioController.Instance.BaseballMiss();
            Debug.Log("BASEBALL MISS");   
            strikes+=1;
            if(strikes == 3){
                hits = 0;
                strikes = 0;
                UIController.Instance.SetHit(hits);
            }
            UIController.Instance.SetStrike(strikes);
        }
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

    public void StartBaseballMatch()
    {
        UIController.Instance.SetState(UIController.UIState.Baseball);
        gameObject.transform.position = new Vector3(Player.transform.position.x+2, Player.transform.position.y, Player.transform.position.z);
        Bat.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        Pitcher.transform.position = new Vector3(Player.transform.position.x+2, Player.transform.position.y, Player.transform.position.z+20);
        Player.GetComponent<PlayerMovement>().cantMove = true;
        CameraController.Instance.FocusOn(gameObject);
        inBaseball = true;
        throwBaseball();

        
    }

    public void throwBaseball()
    {
        Baseball.GetComponent<Renderer>().enabled = true;
        Baseball.transform.position = Pitcher.transform.position;
        Baseball.GetComponent<BaseballPitch>().enabled = false;
        throwing = true;
    }

    public void PitchEnded()
    {
        throwBaseball();
    }

    public void HitBaseball()
    {
        GameObject hitBall = Instantiate(Baseball, hitLocation.position, Quaternion.identity);
        Baseball.GetComponent<Renderer>().enabled = false;
        hitBall.GetComponent<BaseballPitch>().enabled = false;
        Rigidbody rb = hitBall.GetComponent<Rigidbody>();
        if(rb != null)
        {
            Vector3 hitDirection = (transform.forward + Vector3.up).normalized;

            rb.AddForce(hitDirection * 1000f);
            rb.useGravity = true;
        }
    }

     void finishBaseball()
    {
        inBaseball = false;

        Baseball.SetActive(false);
        Bat.SetActive(false);
        UIController.Instance.SetState(UIController.UIState.Exploring);
        Player.GetComponent<PlayerMovement>().cantMove = false;
        CameraController.Instance.FocusOn(Player);
        QuestManager.Instance.ProgressDay();
        gameObject.SetActive(false);

    }
    /*
    IEnumerator MoveActor(GameObject actor, Vector3 position)
    {
        actor.transform.position = position;
        CameraController.Instance.FocusOn(actor);

        yield return new WaitForSeconds(2f);

        CameraController.Instance.FocusOn(Player);
    }*/
    

    //For baseball minigame
        void OnTriggerEnter(Collider entity)
    {
        if(entity.tag == "BASEBALL")
        {
            inPlay = true;
            hitLocation = entity.gameObject.transform;
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
