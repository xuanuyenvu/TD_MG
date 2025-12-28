using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisitModeManager : MonoBehaviour
{
    bool isVisiteGuidee = false;
    public GameObject btn;

    private TextMeshProUGUI btnText;

    void Start()
    {
        btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        isVisiteGuidee = GetVisitMode() == 1;

        if (isVisiteGuidee)
        {
            SetVisiteGuidee();
        }
        else
        {
            SetVisiteLibre();
        }
    }

    public void ChangeVisitMode()
    {
        isVisiteGuidee = !isVisiteGuidee;

        if (isVisiteGuidee)
        {
            SetVisiteGuidee();
        }
        else
        {
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
