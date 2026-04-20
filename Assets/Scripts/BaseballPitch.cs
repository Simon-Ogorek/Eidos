using UnityEngine;

public class BaseballPitch : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;

    public float targetX, targetY, targetZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(10,30);
        targetX = Random.Range(target.position.x - 1f, target.position.x + 1f);
        targetY = Random.Range(target.position.y - 1f, target.position.y + 1f);
        targetZ = target.position.z - 3;

    }

    void OnEnable()
    {
        speed = Random.Range(10,30);
        targetX = Random.Range(target.position.x - 0.5f, target.position.x + 0.5f);
        targetY = Random.Range(target.position.y - 1f, target.position.y + 0.5f);
        targetZ = target.position.z - 3;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, targetY, targetZ), speed*Time.deltaTime);
        if(transform.position == new Vector3(targetX, targetY, targetZ))
            ActivityManager.Instance.PitchEnded();

    }
}
