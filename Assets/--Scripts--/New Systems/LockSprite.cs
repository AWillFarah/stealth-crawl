using UnityEngine;

public class LockSprite : MonoBehaviour
{
    private Quaternion rotation;
    void Start()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = rotation;
    }
}
