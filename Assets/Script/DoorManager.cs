using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Header("Doors")]
    public GameObject leftDoor;
    public GameObject rightDoor;

    [Header("Lights")]
    public List<GameObject> lights;
    public Material openColor;
    public Material closedColor;


    private float moveDistance = 1.8f;
    private float speed = 1f;


    private Vector3 leftClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightClosedPos;
    private Vector3 rightOpenPos;


    private Coroutine leftCoroutine;
    private Coroutine rightCoroutine;

    void Start()
    {
        leftClosedPos = leftDoor.transform.localPosition;
        rightClosedPos = rightDoor.transform.localPosition;


        leftOpenPos = leftClosedPos + Vector3.left * moveDistance;   
        rightOpenPos = rightClosedPos + Vector3.right * moveDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        ChangeWay(true);
        foreach (var light in lights)
        {
            var renderer = light.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = openColor;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ChangeWay(false);
        foreach (var light in lights)
        {
            var renderer = light.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = closedColor;
            }
        }
    }

    private void ChangeWay(bool isOpen)
    {
        if (leftCoroutine != null) 
        {
            StopCoroutine(leftCoroutine); 
        }
        if (rightCoroutine != null) 
        {
            StopCoroutine(rightCoroutine); 
        }

        leftCoroutine = StartCoroutine(MoveDoor(leftDoor.transform, isOpen ? leftOpenPos : leftClosedPos));
        rightCoroutine = StartCoroutine(MoveDoor(rightDoor.transform, isOpen ? rightOpenPos : rightClosedPos));
    }

    private IEnumerator MoveDoor(Transform door, Vector3 targetPos)
    {
        while (Vector3.Distance(door.localPosition, targetPos) > 0.01f)
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }

        door.localPosition = targetPos; 
    }
}
