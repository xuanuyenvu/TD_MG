using UnityEngine; 
using System.Collections;
using TMPro;
using UnityEngine.UI;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DynamicDeformer : MonoBehaviour
{
    public enum Mode { SineBreathing, SculptWithMouse }
    public Mode mode = Mode.SineBreathing;
    [Header("Sine (respiration)")]
    public float amplitude = 0.00008f;
    public float frequency = 0.8f;
    private float normalAmplitude = 0.00008f;
    private float normalFrequency = 0.6f;
    private float performanceAmplitude = 0.0006f;
    private float performanceFrequency = 1.5f;
    private float duration = 7f;

    [Header("Sculpt (souris)")]
    public float radius = 0.2f;
    public float strength = 0.01f;

    [Header("Buttons")]
    public GameObject performanceBtn;
    public GameObject resetBtn;
    // en mètres le long de la normale 
    // Hz 
    // m 
    // m par “coup” de pinceau
    Mesh _mesh;
    Vector3[] _baseVerts;     // copie des vertices d'origine 
    Vector3[] _workVerts;     // buffer de travail 
    Vector3[] _baseNormals;   // normales de référence 
    bool _meshColliderDirty;

    bool isPerformanceMode = false;
    Coroutine performanceCoroutine;
    bool isResetNeeded = false;

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
                Vector3 pushN = hit.normal * dir;
                // Appliquer une bosse/creux gaussienne dans un rayon donn
                for (int i = 0; i < _workVerts.Length; i++)
                {
                    Vector3 worldPos = transform.TransformPoint(_workVerts[i]);
                    float d = Vector3.Distance(worldPos, center);
                    if (d < radius)
                    {
                        float w = Mathf.Exp(-(d * d) / (2f * radius * radius)); // gaussienne
                        worldPos += pushN * (strength * w);
                        _workVerts[i] = transform.InverseTransformPoint(worldPos);

                        isResetNeeded = true;
                        UpdateResetButton();
                    }
                }

                _mesh.vertices = _workVerts;
                _mesh.RecalculateNormals();
                _mesh.RecalculateBounds();

                _meshColliderDirty = true; // on ractualisera plus tard
            }
        }
    }

    public void ChangeMode(bool _isPerformance)
    {
        if (performanceCoroutine != null)
        {
            StopCoroutine(performanceCoroutine);
            performanceCoroutine = null;
        }

        isPerformanceMode = _isPerformance;

        if (isPerformanceMode)
        {
            SetPerformanceMode();
        }
        else
        {
            SetRespirationMode();
        }
    }

    private void SetPerformanceMode()
    {
        TextMeshProUGUI performanceText = performanceBtn.GetComponentInChildren<TextMeshProUGUI>();

        performanceText.text = "<size=60%>MODE</size>\nPerformance";
        performanceCoroutine = StartCoroutine(PlayPerformanceMode(performanceText));
    }

    private void SetRespirationMode()
    {
        performanceBtn.GetComponentInChildren<TextMeshProUGUI>().text = "<size=60%>MODE</size>\nRespiration\n";
        amplitude = normalAmplitude;
        frequency = normalFrequency;
    }

    private IEnumerator PlayPerformanceMode(TextMeshProUGUI performanceText)
    {
        float startAmplitude = amplitude;
        float startFrequency = frequency;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            amplitude = Mathf.Lerp(startAmplitude, performanceAmplitude, t);
            frequency = Mathf.Lerp(startFrequency, performanceFrequency, t);

            UpdatePerformanceCountdown(duration - elapsed, performanceText);
            yield return null;
        }

        amplitude = performanceAmplitude;
        frequency = performanceFrequency;

        yield return new WaitForSeconds(0.5f);

        isPerformanceMode = false;
        performanceCoroutine = null;
        SetRespirationMode();
    }

    private void UpdatePerformanceCountdown(float timeLeft, TextMeshProUGUI performanceText)
    {
        performanceText.text =
            $"<size=60%>MODE</size>\nPerformance\n<size=55%>Time left: {timeLeft:0.0}s</size>";
    }

    public void ResetMesh()
    {
        if (!isResetNeeded) return;
        
        _workVerts = (Vector3[])_baseVerts.Clone();
        _baseVerts = (Vector3[])_baseVerts.Clone();
        _mesh.RecalculateNormals();
        _meshColliderDirty = true;

        isResetNeeded = false;
        UpdateResetButton();
    }

    private void UpdateResetButton()
    {
        resetBtn.GetComponentInChildren<TextMeshProUGUI>().text = isResetNeeded ? "Reset" : "<size=70%>Touch the statue and sculpt!</size>";
    }
}