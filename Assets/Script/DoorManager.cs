using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool way;

    private float moveDistance = 2.56f;
    private float speed = 1f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }


    void Update()
    {
        if (way)
        {
            MoveHorizontal(-1f);
        }
        else
        {
            MoveHorizontal(+1f);
        }
    }

    private void MoveHorizontal(float direction)
    {
        Vector3 targetPosition = initialPosition + Vector3.right * moveDistance * direction;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }

    public void ChangeWay(bool newWay)
    {
        way = newWay;
    }
}
