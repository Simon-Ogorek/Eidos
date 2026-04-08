using UnityEngine;
using Unity.Cinemachine;


public class CameraController : MonoBehaviour
{

    //Cinematic camera deals with zooming in the camera for dialogue or doing cutscenes/changing camera scene.
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
    private CinemachineCamera Camera;

    public static CameraController Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CurrentMember selected;

     void Awake()
    {
        Instance = this;
    }
    
    //FocusOn has the camera look at a certain target.
    public void FocusOn(GameObject Target)
    {
        Camera.Follow = Target.transform;  
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
