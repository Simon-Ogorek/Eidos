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
    }

//Starts a dialogue action from UI Controller
    public void GiveDialogue()
    {
        Debug.Log("This happened" + name);
        i = 0;
        CameraController.Instance.FocusOn(gameObject);
        UIController.Instance.OpenDialogue(dialogue[i], name);
        inDialogue = true;
        player.inDialogue = true;
        i += 1;
    }

    public void EndNPCDialogue()
    {
        inDialogue = false;
        player.inDialogue = false;
        CameraController.Instance.FocusOn(player.gameObject);
        UIController.Instance.EndDialogue();
    }
}