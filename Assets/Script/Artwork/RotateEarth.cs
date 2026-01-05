using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.forward, 20f * Time.deltaTime);
    }
}
