using System.Collections;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool way;

    private float moveDistance = 2.67f;
    private float speed = 1f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }


    IEnumerator MoveDoor(float direction)
    {
        Vector3 targetPosition = initialPosition + Vector3.right * moveDistance * direction;

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        initialPosition = transform.localPosition;
    }



    public void ChangeWay()
    {
        way = !way;
        StartCoroutine(MoveDoor(way ? 1f : -1f));
    }
}
