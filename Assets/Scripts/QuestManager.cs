using UnityEngine;


public class QuestManager : MonoBehaviour
{

    public enum CurrentMember
    {
        Player,
        Eidos
    }


    [SerializeField]
    private GameObject Player;

    [SerializeField]
    private GameObject Eidos;

    [SerializeField]
    private float day;


    public static QuestManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CurrentMember selected;

     void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
    }

    public void StartQuest()
    {
        UIController.Instance.SetQuest("A quest has begun");
    }

    // Update is called once per frame
    void Update()
    {
        //LookTowards(Eidos);
    }
}
