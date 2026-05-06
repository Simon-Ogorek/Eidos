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
    private GameObject focusPoint;

    [SerializeField]
    private CinemachineCamera Camera;

    [SerializeField]
    private CinemachineCamera SceneCamera;
    private CinemachineCamera currentCamera;

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
        currentCamera.Follow = Target.transform;  
    }

    public void LookTowards(GameObject Target)
    {
        focusPoint.transform.position = (Target.transform.position + Player.transform.position) * 0.5f;
        currentCamera.LookAt = focusPoint.transform;  
        //Camera.Transform = Player.transform;
    }

    public void UseSceneCamera()
    {
        Camera.Priority = 0;
        //SceneCamera.Priority = 20;
        currentCamera = SceneCamera;
    }

    public void UseCamera()
    {
        Camera.Priority = 20;
        SceneCamera.Priority = 0;
        currentCamera = Camera;
    }
    void Start()
    {
        UseCamera();
    }

    // Update is called once per frame
    void Update()
    {
        //LookTowards(Eidos);
    }
}
