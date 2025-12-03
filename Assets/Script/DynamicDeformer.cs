using UnityEngine; 
using System;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DynamicDeformer : MonoBehaviour
{
    public enum Mode { SineBreathing, SculptWithMouse }
    public Mode mode = Mode.SineBreathing;
    [Header("Sine (respiration)")]
    public float amplitude = 0.01f;
    public float frequency = 1.0f;
    [Header("Sculpt (souris)")]
    public float radius = 0.25f;
    public float strength = 0.02f;
    // en mètres le long de la normale 
    // Hz 
    // m 
    // m par “coup” de pinceau
    Mesh _mesh;
    Vector3[] _baseVerts;     // copie des vertices d'origine 
    Vector3[] _workVerts;     // buffer de travail 
    Vector3[] _baseNormals;   // normales de référence 
    bool _meshColliderDirty;

    void Awake()
    {
        // .mesh => instance locale dynamique 
        _mesh = GetComponent<MeshFilter>().mesh;
        _mesh.MarkDynamic();

        _baseVerts = _mesh.vertices;
        _workVerts = (Vector3[])_baseVerts.Clone();

        _baseNormals = _mesh.normals;
        if (_baseNormals == null || _baseNormals.Length != _baseVerts.Length)
        {
            _mesh.RecalculateNormals();
            _baseNormals = _mesh.normals;
        }
    }

    void Update()
    {
        if (mode == Mode.SineBreathing)
            DeformSine();
        else
            SculptInput();

        // Mise à jour du mesh (normales recalculées modérément pour limiter le coût) 
        _mesh.vertices = _workVerts;
        if (Time.frameCount % 3 == 0) _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

    // Si vous avez un MeshCollider, ne le mettez à jour qu'en fin de stroke:
        if (_meshColliderDirty && Input.GetMouseButtonUp(0))
        {
            var mc = GetComponent<MeshCollider>();
            if (mc)
            {
                mc.sharedMesh = null;        // détacher avant de réassigner 
                mc.sharedMesh = _mesh;
            }
            _meshColliderDirty = false;
        }
    }

    void DeformSine()
    {
        float t = Time.time * Mathf.PI * 2f * frequency;
        for (int i = 0; i < _workVerts.Length; i++)
        {
            // Déplacement le long de la normale d'origine (respiration douce) 
            float s = Mathf.Sin(t);
            _workVerts[i] = _baseVerts[i] + _baseNormals[i] * (s * amplitude);
        }
    }

    void SculptInput() 
    { 
        if (Input.GetMouseButton(0) && Camera.main) 
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            if (Physics.Raycast(ray, out var hit, 1000f)) 
            { 
                // Sens : clic gauche = pousser, SHIFT+clic gauche = tirer 
                float dir = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? -1f : 1f; 
 
                // Centre et normale au point d'impact (espace monde) 
                Vector3 center = hit.point; 
                Vector3 pushN  = hit.normal * dir; 
 
                // Appliquer une bosse/creux gaussienne dans un rayon donné 
                for (int i = 0; i < _workVerts.Length; i++) 
                { 
                    Vector3 worldPos = transform.TransformPoint(_workVerts[i]); 
                    float d = Vector3.Distance(worldPos, center); 
                    if (d < radius) 
                    { 
                        float w = Mathf.Exp(-(d * d) / (2f * radius * radius)); // gaussienne 
                        worldPos += pushN * (strength * w); 
                        _workVerts[i] = transform.InverseTransformPoint(worldPos); 
                    } 
                } 
                _meshColliderDirty = true; // on réactualisera plus tard 
            } 
        } 
    } 
}