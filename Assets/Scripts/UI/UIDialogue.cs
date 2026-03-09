using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{
        
        public GameObject panel;
        public TMP_Text dialogue;



    public void SetDialogue(string dialogueInput)
    {
        dialogue.text = dialogueInput;
    }

}
