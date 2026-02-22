using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NOT IMPLEMENTED | TODO
/// </summary>
public class UIStatusBar : MonoBehaviour
{
    [SerializeField]
    private Color lowStatusColor;
    [SerializeField]
    private float lowStatusPercentage;
    [SerializeField]
    private GameObject fillObject; 

    private Color originalColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalColor = fillObject.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void UpdateStatusBar(float valuePercentage)
    {
        Image fillImage = fillObject.GetComponent<Image>();
        if (valuePercentage < lowStatusPercentage)
        {
            fillImage.color = lowStatusColor;
        }
        else
        {
            fillImage.color = originalColor;
        }
    }
}
