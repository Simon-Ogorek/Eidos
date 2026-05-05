using UnityEngine;


public class AudioController : MonoBehaviour
{


    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip adventureMusic;
    [SerializeField] private AudioClip combatMusic;


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
 
    void Start()
    {
        PlayMusic(adventureMusic);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
