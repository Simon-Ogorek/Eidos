using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// Entities that are noncombatants
/// </summary>
public class NonCombatant : MonoBehaviour
{

//Array with all of the dialogue of the NPC
    [SerializeField]
    public string[] dialogue;

    [SerializeField]
    public string name;

    public PlayerMovement player;

    int i = 0;
    bool inDialogue = false;

    bool questDialogue = false;
    bool usingController = false;

    float questDialogueStartTime;

    float normalDialogueStartTime;

    private float blockNormalDialogueUntil = 0f;

    

    void Start()
    {
        
    }

//Goes through dialogue array
    void Update()
    {
        if(Gamepad.current!=null){
            usingController = true;
        }
        else if(Gamepad.current==null)
        {
            usingController = false;
        }
        if(inDialogue)
        {
        if(i < dialogue.Length)
        {
            Debug.Log("Dialogue loop" + name);
            if(Input.GetKeyDown(KeyCode.I) || (usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                Debug.Log(name + " dialogueI");
                if(i <= dialogue.Length-1){
                    if(dialogue[i] == "STARTQUEST")
                        {
                            EndNPCDialogue();
                            QuestManager.Instance.StartQuest();

                        }
                    else if(dialogue[i] == "CONTINUEQUEST")
                        {
                            EndNPCDialogue();
                            QuestManager.Instance.ContinueQuest();

                    }
                    else if(dialogue[i] == "BASEBALL")
                        {
                            EndNPCDialogue();
                            QuestManager.Instance.PlayBaseball(gameObject);

                    }
                    else if(dialogue[i] == "ENEMY")
                        {
                            EndNPCDialogue();
                            becomeEnemy();
                    }
                    else
                        {
                            AudioController.Instance.PlayInteract();
                            Debug.Log("Dialogue for" + name);
                            UIController.Instance.OpenDialogue(dialogue[i], name);
                        }
                }
                i+=1;
                Debug.Log("The number is" + i);
            }
        }
        else
            {
                EndNPCDialogue();
            }
        }
        else if (questDialogue)
        {
            if(Time.time - questDialogueStartTime < 0.2f)
                return;
            if(Input.GetKeyDown(KeyCode.I) || (usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                Debug.Log(name + " quest dialogue interaction");
                questDialogue = false;
                CameraController.Instance.FocusOn(player.gameObject);
                UIController.Instance.EndDialogue();
                QuestManager.Instance.ContinueQuest();
                player.cantMove = false;

                blockNormalDialogueUntil = Time.time + 0.25f;
            }
            
        }
    }

//Starts a dialogue action from UI Controller
    public void GiveDialogue()
    {
        if(Time.time - normalDialogueStartTime < 0.2f)
            return;

        if(Time.time < blockNormalDialogueUntil)
            return;
            
        AudioController.Instance.PlayInteract();
        Debug.Log("This happened" + name + dialogue[i]);
        
        while (i < dialogue.Length && dialogue[i].StartsWith("QUEST"))
        {
            i++;
        }

        if(i >= dialogue.Length)
            return;

        if(dialogue[i] == "STARTQUEST")
        {
            EndNPCDialogue();
            QuestManager.Instance.StartQuest();
            i+=1;
            return;
        }
        if(dialogue[i] == "CONTINUEQUEST")
        {
            EndNPCDialogue();
            QuestManager.Instance.ContinueQuest();
            i+=1;
            return;
        }
        if(dialogue[i] == "BASEBALL")
        {
            EndNPCDialogue();
            QuestManager.Instance.PlayBaseball(gameObject);
            i+=1;
            return;
        }
        if(dialogue[i] == "ENEMY")
        {
            EndNPCDialogue();
            becomeEnemy();
        }
        else{
        CameraController.Instance.FocusOn(gameObject);
        UIController.Instance.OpenDialogue(dialogue[i], name);
        inDialogue = true;
        normalDialogueStartTime = Time.time;
        player.cantMove = true;
        i += 1;}
    }

    public void GiveQuestDialogue(string dialogue, string type = "Dialogue")
    {
        inDialogue = false;
        AudioController.Instance.PlayInteract();
        questDialogue = true;
        questDialogueStartTime = Time.time;

        player.cantMove = true;
        UIController.Instance.OpenDialogue(dialogue, name, type);
    }

    public void EndNPCDialogue()
    {
        inDialogue = false;

       // if(i >= 2)
       //     i-=2;
        //else
        //    i = 0;
        player.cantMove = false;
        CameraController.Instance.FocusOn(player.gameObject);
        UIController.Instance.EndDialogue();
    }

    public void AdvanceDialogueGroup(string marker)
    {
        i = 0;
        while(i < dialogue.Length && dialogue[i] != marker)
            i++;

        if(i >= dialogue.Length)
        {
            Debug.LogWarning(name + "couldn't find dialogue marker: " + marker);
            return;
        }

        i+=1;
    }

    public void becomeEnemy()
    {
        gameObject.tag = "Enemy";
        Enemy enemy = gameObject.GetComponent<Enemy>();

        if(enemy != null){
        enemy.enabled = true;
        enemy.isEnemy = true;

        UIController.Instance.AddToEnemyPanel(enemy);
        UIController.Instance.playerCombatant.target = enemy;
        }
        
        this.enabled = false;
    }
}