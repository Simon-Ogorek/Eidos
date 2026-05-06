using System;
using System.Collections.Generic;
using UnityEngine;


public class AudioController : MonoBehaviour
{


    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip adventureMusic;
    [SerializeField] private AudioClip combatMusic;

    [SerializeField] private AudioClip interactSound;
    [SerializeField] private AudioClip popupSound;

    [SerializeField] private AudioClip combatMoveUp;
    [SerializeField] private AudioClip combatMoveDown;
    [SerializeField] private AudioClip combatSelectUp;
    [SerializeField] private AudioClip combatSelectDown;
    [SerializeField] private AudioClip combatHurt;
    [SerializeField] private AudioClip combatWin;

    
    public static AudioController Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created


     void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx);
    }

    public void PlayCombatMusic()
    {
        PlayMusic(combatMusic);
    }

    public void PlayAdventureMusic()
    {
        PlayMusic(adventureMusic);
    }

    public void PlayPopUp()
    {
        PlaySFX(popupSound);
    }

    public void PlayInteract()
    {
        PlaySFX(interactSound);
    }
 
    void Start()
    {
        PlayMusic(adventureMusic);
    }

    public void BattlePlayMoveUp()
    {
        PlaySFX(combatMoveUp);
    }

    public void BattlePlayMoveDown()
    {
        PlaySFX(combatMoveDown);
    }

    public void BattlePlaySelectUp()
    {
        PlaySFX(combatSelectUp);
    }

        public void BattlePlaySelectDown()
    {
        PlaySFX(combatSelectDown);
    }
        public void BattlePlayHurt()
    {
        PlaySFX(combatHurt);
    }    public void BattlePlayWin()
    {
        PlaySFX(combatWin);
}
}
