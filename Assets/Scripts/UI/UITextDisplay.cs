using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITextDisplay : MonoBehaviour
{
        
        public GameObject panel;
        public TMP_Text textBox;

        public TMP_Text header;



    public void SetText(string textInput)
    {
        textBox.text = textInput;
    }   

    //character name for dialogue, quest header for quest, etc
    public void SetHeader(string textInput)
    {
        header.text = textInput;
    }   

}
