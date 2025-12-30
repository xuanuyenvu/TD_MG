using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 20f * Time.deltaTime);
    }
}
