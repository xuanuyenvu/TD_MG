using UnityEngine;

public class LightManager : MonoBehaviour
{
    private GameObject character;
    private Light lightSpot;
    private MeshRenderer meshNeon;

    private float factorSpot = 100f;
    private float maxIntensity = 16f;
    private float minIntensity = 0f;
    private float factorNeon = 3f;

    void Start()
    {
        character = GameObject.FindWithTag("Player");
        lightSpot = GetComponentInChildren<Light>();
        meshNeon = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
    {
        ModifyLightIntensity();
    }

    private float CalculateDistance()
    {
        return Vector3.Distance(transform.position, character.transform.position);
    }

    private void ModifyLightIntensity()
    {
        float distance = CalculateDistance();
        lightSpot.intensity = Mathf.Clamp(factorSpot / (distance * distance * distance), minIntensity, maxIntensity);

        Color finalEmissionColor = Color.white * Mathf.LinearToGammaSpace(lightSpot.intensity / factorNeon);
        meshNeon.material.SetColor("_EmissionColor", finalEmissionColor);
    }
}
