using UnityEngine;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Just for Midterm Splash Screen
/// </summary>
public class TitleSplash : MonoBehaviour
{

    public GameObject background;
    public GameObject StudioEidos;
    
    public GameObject LifeOfP;
    public GameObject Tessemark;
    int i = 0;

    void Start()
    {
        background.SetActive(true);
        StudioEidos.SetActive(false);
        LifeOfP.SetActive(false);
        Tessemark.SetActive(false);
    }

    void Update()
    {
        i+=1;
        if(i==50)
            StudioEidos.SetActive(true);
        if(i==500)
            StudioEidos.SetActive(false);
        if(i==550)
            Tessemark.SetActive(true);
        if(i==600)
            LifeOfP.SetActive(true);
        if (i == 1200)
        {
            LifeOfP.SetActive(false);
            Tessemark.SetActive(false);
            background.SetActive(false);
            this.enabled = false;
        }
    }

}
