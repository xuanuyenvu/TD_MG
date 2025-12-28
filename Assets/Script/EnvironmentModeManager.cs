using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnvironmentModeManager : MonoBehaviour
{
    public GameObject btn;

    [Header("Colors")]
    private Light lightMode;
    public Color calmColor;
    public List<Color> eventColors;

    [Header("Music")]
    private AudioSource audioSource;
    public AudioClip calmMusic;
    public AudioClip eventMusic;

    public bool isEvent = true;

    private int currentEventIndex;
    private float colorChangeSpeed = 2f;

    private TextMeshProUGUI btnText;
    private Image btnImage;

    private Coroutine eventCoroutine;

    void Start()
    {
        lightMode = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        btnImage = btn.GetComponent<Image>();

        if (isEvent)
        {
            StartEventMode();
        }
        else
        {
            SetCalmMode();
        }
    }

    public void ChangeMode()
    {
        isEvent = !isEvent;

        if (isEvent)
            StartEventMode();
        else
            SetCalmMode();
    }

    private void SetCalmMode()
    {
        if (eventCoroutine != null)
            StopCoroutine(eventCoroutine);

        btnText.text = "<size=60%>MODE</size>\nCalme";

        lightMode.color = calmColor;

        Color c = calmColor;
        c.a = 1f;
        btnImage.color = c;

        if (audioSource.clip != calmMusic)
        {
            audioSource.clip = calmMusic;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }
    }

    private void StartEventMode()
    {
        if (eventColors == null || eventColors.Count < 2) return;

        btnText.text = "<size=60%>MODE</size>\nÉvénement";

        if (audioSource.clip != eventMusic)
        {
            audioSource.clip = eventMusic;
            audioSource.volume = 0.8f;
            audioSource.Play();
        }

        eventCoroutine = StartCoroutine(EventModeCoroutine());
    }

    private IEnumerator EventModeCoroutine()
    {
        while (true)
        {
            Color from = eventColors[currentEventIndex];
            Color to = eventColors[(currentEventIndex + 1) % eventColors.Count];

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * colorChangeSpeed;

                Color c = Color.Lerp(from, to, t);
                c.a = 1f;

                lightMode.color = c;
                btnImage.color = c;

                yield return null;
            }

            currentEventIndex = (currentEventIndex + 1) % eventColors.Count;
        }
    }
}