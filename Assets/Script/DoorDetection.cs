using UnityEngine;

public class DoorDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        foreach (DoorManager door in GetComponentsInChildren<DoorManager>())
        {
            door.ChangeWay();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        foreach (DoorManager door in GetComponentsInChildren<DoorManager>())
        {
            door.ChangeWay();
        }
    }
}
