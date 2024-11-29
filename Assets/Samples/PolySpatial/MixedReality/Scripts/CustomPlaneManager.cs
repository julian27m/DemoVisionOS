using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CustomPlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;

    void Start()
    {
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
        }

        // Inicializar los planos ocultando sus mallas pero manteniendo los colliders activos
        foreach (var plane in planeManager.trackables)
        {
            var customVisualizer = plane.GetComponent<CustomPlaneVisualizer>();
            if (customVisualizer != null)
            {
                customVisualizer.SetMeshVisibility(false); // Ocultar la malla
                customVisualizer.EnableCollider(true);    // Mantener el collider activo
            }
        }
    }

    public void TogglePlaneMeshes(bool showMeshes)
    {
        foreach (var plane in planeManager.trackables)
        {
            var customVisualizer = plane.GetComponent<CustomPlaneVisualizer>();
            if (customVisualizer != null)
            {
                customVisualizer.SetMeshVisibility(showMeshes);
            }
        }
    }
}
