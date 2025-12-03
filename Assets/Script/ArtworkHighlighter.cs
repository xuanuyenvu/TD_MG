using UnityEngine;
public class ArtworkHighlighter : MonoBehaviour
{
    [Header("Cibles")]
    [SerializeField] Renderer targetRenderer;          // l’œuvre (option A: Fresnel en 2e matériel) 
    [SerializeField] Transform haloShell;              // option B: l’enfant “Artwork_HaloShell” 
    [SerializeField] ParticleSystem auraParticles;
    [Header("Shader property (option A)")]
    [SerializeField] string alphaProp = "_BaseAlpha";  // doit matcher la prop du graph
    [SerializeField] float onAlpha = 0.6f, offAlpha = 0.0f;
    MaterialPropertyBlock _mpb;
    void Awake()
    {
        if (!targetRenderer) targetRenderer =
        GetComponentInChildren<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }
    public void SetHighlight(bool on)
    {
        // Option A: Fresnel par MaterialPropertyBlock (sur le 2e matériau) 
        if (targetRenderer)
        {
        }
        targetRenderer.GetPropertyBlock(_mpb, 1); // slot 1 = 2e matériau 
        _mpb.SetFloat(alphaProp, on ? onAlpha : offAlpha);
        targetRenderer.SetPropertyBlock(_mpb, 1);
        // Option B: la coquille 
        if (haloShell) haloShell.gameObject.SetActive(on);
        // Particules 
        if (auraParticles)
        {
            if (on && !auraParticles.isPlaying) auraParticles.Play();
            if (!on && auraParticles.isPlaying) auraParticles.Stop(true,
            ParticleSystemStopBehavior.StopEmitting);
        }
    }
}