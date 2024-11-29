using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

#if UNITY_INCLUDE_ARFOUNDATION
using UnityEngine.XR.ARFoundation;
#endif

namespace PolySpatial.Samples
{
    public class ChangeOnInput : MonoBehaviour
    {
        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        // Variable para rastrear el estado de los toques previos
        private bool hasActiveTouches = false;

        void Update()
        {
            // Verifica si hay toques activos
            int touchCount = Touch.activeTouches.Count;

            // Solo imprime el mensaje cuando hay un cambio en el estado de los toques
            if (touchCount > 0 && !hasActiveTouches)
            {
                hasActiveTouches = true; // Cambia el estado a "hay toques"
                Debug.Log($"Toques activos detectados: {touchCount}");
            }
            else if (touchCount == 0 && hasActiveTouches)
            {
                hasActiveTouches = false; // Cambia el estado a "no hay toques"
                Debug.Log("No se detectan toques activos.");
            }

            // Procesa los toques activos
            if (touchCount > 0)
            {
                foreach (var touch in Touch.activeTouches)
                {
                    // Solo procesa el evento si el toque está en la fase Began
                    if (touch.phase == TouchPhase.Began)
                    {
                        // Obtén los datos del puntero espacial para el toque
                        SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);

                        if (touchData.targetObject != null)
                        {
                            // Log del objeto tocado
                            Debug.Log($"Objeto detectado por el toque: {touchData.targetObject.name}");

                            // Alterna la visibilidad del mesh render del objeto detectado
                            ToggleObjectMeshVisibility(touchData.targetObject);

                            // Si el objeto tiene PlaneDataUI, obtén su clasificación y alineación
                            ExtractPlaneData(touchData.targetObject);

                            // Opcional: Detener el loop después del primer toque procesado
                            break;
                        }
                    }
                }
            }
        }

        // Alterna la visibilidad del MeshRenderer
        void ToggleObjectMeshVisibility(GameObject obj)
        {
            MeshRenderer objRenderer = obj.GetComponent<MeshRenderer>();
            if (objRenderer != null)
            {
                objRenderer.enabled = !objRenderer.enabled; // Alternar estado actual
                Debug.Log($"Objeto {obj.name} - Visibilidad cambiada a: {objRenderer.enabled}");
            }
            else
            {
                Debug.LogWarning($"El objeto {obj.name} no tiene un componente MeshRenderer.");
            }
        }

        void ExtractPlaneData(GameObject obj)
        {
            PlaneDataUI planeData = obj.GetComponent<PlaneDataUI>();
            if (planeData != null)
            {
                // Accede a las propiedades públicas Classification y Alignment
                string classification = planeData.Classification;
                string alignment = planeData.Alignment;

                Debug.Log($"The selected object is a {classification} and it is {alignment} oriented.");
            }
            else
            {
                Debug.Log("El objeto no tiene el componente PlaneDataUI.");
            }
        }


        // Cambia el color del objeto (si aún lo necesitas)
        void ChangeObjectColor(GameObject obj)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material.color = new Color(Random.value, Random.value, Random.value);
            }
        }
    }
}