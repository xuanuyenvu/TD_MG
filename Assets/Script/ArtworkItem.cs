using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ArtworkItem : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Material fresnelMaterial;
    [SerializeField] private GameObject description;
    [TextArea(3,10)]  
    [SerializeField] private string artworkDescription;

    void Start()
    {
        description.GetComponentInChildren<TextMeshProUGUI>().text = artworkDescription;
    }

    public void SetFresnelEnabled(bool enabled)
    {
        foreach (Renderer rend in renderers)
        {
            Material[] materials = rend.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains("Fresnel"))
                {
                    materials[i] = enabled ? fresnelMaterial : rend.sharedMaterials[i];
                }
            }
            rend.materials = materials;
        }
    }
}
