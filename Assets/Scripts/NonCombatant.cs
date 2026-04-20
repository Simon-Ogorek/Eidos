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
        if(i <= dialogue.Length)
        {
            Debug.Log("Dialogue loop" + name);
            if(Input.GetKeyDown(KeyCode.I) || (usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                Debug.Log(name + " dialogueI");
                if(i <= dialogue.Length-1){
                    if(dialogue[i] == "QUEST")
                        {
                            EndNPCDialogue();
                            QuestManager.Instance.StartQuest();

                        }
                    else if(dialogue[i] == "CONTINUEQUEST")
                        {
                            EndNPCDialogue();
                            QuestManager.Instance.ContinueQuest();

                    }
                    else
                        {
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
            if(Input.GetKeyDown(KeyCode.I) || (usingController && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                Debug.Log(name + " quest dialogue interaction");
                questDialogue = false;
                CameraController.Instance.FocusOn(player.gameObject);
                UIController.Instance.EndDialogue();
                QuestManager.Instance.ContinueQuest();
            }
            
        }
    }

//Starts a dialogue action from UI Controller
    public void GiveDialogue()
    {
        Debug.Log("This happened" + name);
        CameraController.Instance.FocusOn(gameObject);
        UIController.Instance.OpenDialogue(dialogue[i], name);
        inDialogue = true;
        player.cantMove = true;
        i += 1;
    }

    public void GiveQuestDialogue(string dialogue)
    {
        questDialogue = true;
        UIController.Instance.OpenDialogue(dialogue, name);
    }

    public void EndNPCDialogue()
    {
        inDialogue = false;
        player.cantMove = false;
        CameraController.Instance.FocusOn(player.gameObject);
        UIController.Instance.EndDialogue();
    }

    public void AdvanceDialogueGroup(string marker)
    {
        while(dialogue[i] != marker)
            i++;
        i+=1;
    }
}