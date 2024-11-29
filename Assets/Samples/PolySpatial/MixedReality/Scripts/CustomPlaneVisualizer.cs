using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CustomPlaneVisualizer : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private ARPlaneMeshVisualizer originalVisualizer;

    void Awake()
    {
        // Buscar componentes relacionados al plano
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        originalVisualizer = GetComponent<ARPlaneMeshVisualizer>();

        if (originalVisualizer != null)
        {
            // Desactivar el ARPlaneMeshVisualizer para tomar control manual
            originalVisualizer.enabled = false;
        }
    }

    public void SetMeshVisibility(bool isVisible)
    {
        if (meshRenderer != null)
        {
            meshRenderer.enabled = isVisible;
        }
    }

    public void EnableCollider(bool isEnabled)
    {
        if (meshCollider != null)
        {
            meshCollider.enabled = isEnabled;
        }
    }
}
