using UnityEngine;

public class CameraTargeting : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform cameraTrackPoint;
    public float cameraRotationSpeed;
    public float cameraMovementSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTarget)
        {
            Vector3 lookDirection = (cameraTarget.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, cameraRotationSpeed * Time.deltaTime);
        }
        if (cameraTrackPoint)
        {
            transform.position = Vector3.Slerp(transform.position, cameraTrackPoint.position, cameraMovementSpeed * Time.deltaTime);
        }
    }
}
