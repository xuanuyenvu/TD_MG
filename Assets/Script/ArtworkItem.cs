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

    private Dictionary<Renderer, Material[]> originalMaterials = new();
    private bool isActive = false;
    public bool wantActive = false;

    void Start()
    {
        description.GetComponentInChildren<TextMeshProUGUI>().text = artworkDescription;
        foreach (var r in renderers)
        {
            originalMaterials[r] = r.materials;
        }

        particle.Stop();
        description.SetActive(false);
    }

    void Update()
    {
        // if (wantActive)
        // {
        //     SetActiveArtwork(true);
        // }
        // else
        // {
        //     SetActiveArtwork(false);
        // }
    }

   public void SetActiveArtwork(bool active)
    {
        if (isActive == active) return; 

        isActive = active;

        if (isActive)
            ActiveArtwork();
        else
            DeactiveArtwork();
    }

    public void ActiveArtwork()
    {
        particle.Play();
        description.SetActive(true);
        ActiverHighlight();
    }

    public void DeactiveArtwork()
    {
        particle.Stop();
        description.SetActive(false);
        DesactiverHighlight();
    }

    private void ActiverHighlight()
    {
        foreach (var r in renderers)
        {
            var mats = new List<Material>(originalMaterials[r]);
            if (fresnelMaterial != null)
            {
                mats.Add(fresnelMaterial);
            }
            r.materials = mats.ToArray();
        }
    }

    private void DesactiverHighlight()
    {
        foreach (var r in renderers)
        {
            r.materials = originalMaterials[r]; 
        }
    }

    public Vector3 GetPosition()
    {
        return transform.Find("NPC").transform.position;
    }

    public Vector3 GetRotation()
    {
        return transform.Find("NPC").transform.eulerAngles;
    }
}
