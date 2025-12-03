using Invector.vCharacterController;
using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    private Vector3 posInit;
    private vThirdPersonInput vThirdPersonInput;
    void Start()
    {
        posInit = GetComponent<Transform>().position;
        vThirdPersonInput = GetComponent<vThirdPersonInput>();
    }

    public void SetInitialPosition()
    {
        StartCoroutine(ResetPositionCoroutine());
    }

    private IEnumerator ResetPositionCoroutine()
    {
        vThirdPersonInput.enabled = false;
        transform.position = posInit;

        yield return new WaitForSeconds(0.1f);
        vThirdPersonInput.enabled = true;
    }

}
