using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Entities that are noncombatants
/// </summary>
public class NonCombatant : MonoBehaviour
{


    [SerializeField]
    protected Transform player;

    protected string[] dialogue = {"Hello there", "How do you do"};

    int test = 0; 

    void Start()
    {
        
    }
    void Update()
    {

    }

    public void GiveDialogue()
    {
        int i = 0;
        while(i < dialogue.Length)
        {
            UIController.Instance.OpenDialogue(dialogue[i]);
            if(Input.GetKeyDown(KeyCode.U))
            {
                i+=1;
            }
        }
    }
}