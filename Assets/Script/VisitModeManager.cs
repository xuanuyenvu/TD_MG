using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisitModeManager : MonoBehaviour
{
    public GameObject btn;
    public GameObject artworkButtons;
    private NPCManager npcManager;

    private bool IsVisiteGuidee = false;
    private TextMeshProUGUI btnText;


    void Start()
    {
        npcManager = FindFirstObjectByType<NPCManager>();
        btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        IsVisiteGuidee = GetVisitMode() == 1;

        if (IsVisiteGuidee)
        {
            npcManager.StartGuidedTour();
            SetVisiteGuidee();
        }
        else
        {
            SetVisiteLibre();
        }
    }

    public void ChangeVisitMode()
    {
        IsVisiteGuidee = !IsVisiteGuidee;

        if (IsVisiteGuidee)
        {
            npcManager.StartGuidedTour();
            SetVisiteGuidee();
        }
        else
        {
            npcManager.StopGuidedTour();
            SetVisiteLibre();
        }
    }

    private void SetVisiteLibre()
    {
        artworkButtons.SetActive(true);
        btnText.text = "Visite libre";
        PlayerPrefs.SetInt("VisitMode", 0);
        PlayerPrefs.Save();
    }

    private void SetVisiteGuidee()
    {
        artworkButtons.SetActive(false);
        btnText.text = "Visite guidée\npar PNJ";
        PlayerPrefs.SetInt("VisitMode", 1);
        PlayerPrefs.Save();
    }

    public static int GetVisitMode()
    {
        return PlayerPrefs.GetInt("VisitMode", 0);
    }
}
