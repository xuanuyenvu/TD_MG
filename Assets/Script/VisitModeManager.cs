using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisitModeManager : MonoBehaviour
{
    public static VisitModeManager instance;
    public GameObject btn;

    public bool IsVisiteGuidee { get; private set; } = false;
    private TextMeshProUGUI btnText;

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
        btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        IsVisiteGuidee = GetVisitMode() == 1;

        if (IsVisiteGuidee)
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
        IsVisiteGuidee = !IsVisiteGuidee;

        if (IsVisiteGuidee)
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
