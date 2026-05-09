using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

/// <summary>
/// Just for Midterm Splash Screen
/// </summary>
public class TitleSplash : MonoBehaviour
{

    public GameObject background;
    public GameObject StudioEidos;
    
    public GameObject LifeOfP;

    public GameObject questManager;

    public GameObject fadeBackground;
    int i = 0;

    void Start()
    {
        background.SetActive(true);
        StudioEidos.SetActive(false);
        LifeOfP.SetActive(false);

        questManager.SetActive(false);


    }

    void Update()
    {
        i+=1;
        if(i==50)
            StudioEidos.SetActive(true);
        if(i==500)
            StudioEidos.SetActive(false);
        if(i==600)
            LifeOfP.SetActive(true);
        if (i == 1200)
        {
            StartCoroutine(EndSplash());
        }
    }

    IEnumerator EndSplash()
    {
        this.enabled = false;

        LifeOfP.SetActive(false);
        background.SetActive(false);

        questManager.SetActive(true);
        fadeBackground.SetActive(true);

        

        yield return StartCoroutine(UIController.Instance.FadeIn());
    }

}
