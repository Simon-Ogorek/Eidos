using UnityEngine;

public class DoRandomShit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = new Vector3(Random.Range(0,90), Random.Range(0,90), Random.Range(0,90));
        transform.Rotate(rotation * Time.deltaTime * 100);
    }
}
