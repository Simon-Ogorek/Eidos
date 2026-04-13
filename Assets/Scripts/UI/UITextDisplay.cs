using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITextDisplay : MonoBehaviour
{
        
        public GameObject panel;
        public TMP_Text textBox;



    public void SetText(string textInput)
    {
        textBox.text = textInput;
    }

}
