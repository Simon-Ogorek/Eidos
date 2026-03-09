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

    int test = 0; 

    void Start()
    {
        
    }
    void Update()
    {
        if(test==0){
        UIController.Instance.OpenDialogue("Hello there!");
        test+=1;
        }
        if(test==100){
        UIController.Instance.OpenDialogue("I am the king!");
        test+=1;
        }
        test+=1;
    }
}