using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionController : MonoBehaviour
{
    [SerializeField]
    private ARPlaneManager planeManager;

    [Tooltip("Controla si la detección de planos está activa o no.")]
    private bool enablePlaneDetection = true;

    void Start()
    {
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
        }

        if (planeManager == null)
        {
            Debug.LogError("No ARPlaneManager found in the scene.");
            enabled = false;
            return;
        }

        UpdatePlaneDetection();
    }

    public void TogglePlaneDetection()
    {
        enablePlaneDetection = !enablePlaneDetection;
        UpdatePlaneDetection();
    }

    private void UpdatePlaneDetection()
    {
        planeManager.enabled = enablePlaneDetection;

        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(enablePlaneDetection);
        }

        Debug.Log($"Plane detection is now {(enablePlaneDetection ? "enabled" : "disabled")}.");
    }
}
