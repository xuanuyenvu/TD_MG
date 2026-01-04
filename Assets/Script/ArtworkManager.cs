using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtworkManager : MonoBehaviour
{
    public static ArtworkManager instance;
    [SerializeField] private List<ArtworkItem> artworkItems;
    [SerializeField] private Button previousBtn;
    [SerializeField] private Button nextBtn;
    private GameObject player;

    public int currentIndex { get; private set; } = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public Vector3 GetArtworkPosition(int index)
    {
        if (index < 0 || index >= artworkItems.Count) return Vector3.zero;
        return artworkItems[index].GetPosition();
    }

    public Vector3 GetArtworkRotation(int index)
    {
        if (index < 0 || index >= artworkItems.Count) return Vector3.zero;
        return artworkItems[index].GetRotation();
    }

    public void ResetTour()
    {
        artworkItems[currentIndex].SetActiveArtwork(false);
        currentIndex = 0;
        artworkItems[currentIndex].SetActiveArtwork(true);
        UpdateStateButtons();
    }
}
