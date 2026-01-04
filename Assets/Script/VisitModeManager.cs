using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisitModeManager : MonoBehaviour
{
    public GameObject btn;
    private NPCManager npcManager;

    public bool IsVisiteGuidee { get; private set; } = false;
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
        btnText.text = "Visite libre";
        PlayerPrefs.SetInt("VisitMode", 0);
        PlayerPrefs.Save();
    }

    private void SetVisiteGuidee()
    {
        btnText.text = "Visite guidée\npar PNJ";
        PlayerPrefs.SetInt("VisitMode", 1);
        PlayerPrefs.Save();
    }

    public static int GetVisitMode()
    {
        return PlayerPrefs.GetInt("VisitMode", 0);
    }
}
