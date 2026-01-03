using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtworkManager : MonoBehaviour
{
    [SerializeField] private List<ArtworkItem> artworkItems;
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;
    private GameObject player;

    private int currentIndex = 0;

    void Start()
    {
        artworkItems = new List<ArtworkItem>(GetComponentsInChildren<ArtworkItem>());
        player = GameObject.FindGameObjectWithTag("Player");

        foreach (var item in artworkItems)
        {
            item.SetActiveArtwork(false);
        }

        if (artworkItems.Count > 0)
        {
            currentIndex = 0;
            artworkItems[currentIndex].SetActiveArtwork(true);
        }
        UpdateStateButtons();
    }

    public void NextArtwork()
    {
        artworkItems[currentIndex].SetActiveArtwork(false);

        currentIndex++;

        artworkItems[currentIndex].SetActiveArtwork(true);

        UpdateStateButtons();
    }

    public void PreviousArtwork()
    {
        artworkItems[currentIndex].SetActiveArtwork(false);

        currentIndex--;

        artworkItems[currentIndex].SetActiveArtwork(true);

        UpdateStateButtons();
    }

    private void UpdateStateButtons()
    {
        previousBtn.interactable = currentIndex > 0;
        nextBtn.interactable = currentIndex < artworkItems.Count - 1;
    }

}
