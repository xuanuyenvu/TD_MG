using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    private float minDistanceNPCandPlayer = 3f;
    private Vector3 initialPosition;
    [SerializeField] private TextMeshProUGUI text;
    [TextArea(3,10)]
    [SerializeField] private List<string> artworkDescription;

    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if (!VisitModeManager.instance.IsVisiteGuidee) return;


    }


}
