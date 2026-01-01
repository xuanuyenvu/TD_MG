using System.Collections.Generic;
using UnityEngine;

public class ArtworkManager : MonoBehaviour
{
    [SerializeField] private List<ArtworkItem> artworkItems;

    void Start()
    {
        artworkItems = new List<ArtworkItem>(GetComponentsInChildren<ArtworkItem>());
    }
}
