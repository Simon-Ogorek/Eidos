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
    
    //SwitchMember switches player control from player character to eidos partner.
    public void GivingDialogue(GameObject Speaker)
    {

        Camera.Follow = Speaker.transform;    
    } 

    public void FocusOnPlayer(GameObject Player)
    {
        Camera.Follow = Player.transform;  
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
