using Unity.PolySpatial.InputDevices;
using UnityEngine;
using System.Collections;
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
        [SerializeField]
        Material selected_material;

        [SerializeField]
        Material default_material;


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
                            //ToggleObjectMeshVisibility(touchData.targetObject);
                            ChangeObjectMaterial(touchData.targetObject);

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
            // Verifica si el objeto tiene un componente PlaneDataUI directamente
            PlaneDataUI planeData = obj.GetComponent<PlaneDataUI>();

            if (planeData == null)
            {
                // Si no lo tiene, busca el componente PlaneDataUI en el objeto padre
                if (obj.transform.parent != null)
                {
                    planeData = obj.transform.parent.GetComponent<PlaneDataUI>();
                }
            }

            if (planeData != null)
            {
                // Accede a las propiedades públicas Classification y Alignment
                string classification = planeData.Classification;
                string alignment = planeData.Alignment;

                Debug.Log($"The selected object is a {classification} and it is {alignment} oriented.");
            }
            else
            {
                Debug.Log("No se encontró el componente PlaneDataUI ni en el objeto ni en su padre.");
            }
        }



        void ChangeObjectMaterial(GameObject obj)
        {
            // Obtén el objeto padre del objeto con el que se está interactuando
            Transform parentTransform = obj.transform.parent;

            if (parentTransform != null)
            {
                Renderer parentRenderer = parentTransform.GetComponent<Renderer>();
                if (parentRenderer != null)
                {
                    // Cambia el material al seleccionado
                    parentRenderer.material = selected_material;
                    Debug.Log($"El material del objeto {parentTransform.name} se cambió al material seleccionado.");

                    // Inicia la corrutina para volver al material original después de 5 segundos
                    StartCoroutine(RevertMaterialAfterDelay(parentRenderer, 5f));
                }
                else
                {
                    Debug.LogWarning($"El objeto padre {parentTransform.name} no tiene un componente Renderer.");
                }
            }
            else
            {
                Debug.LogWarning($"El objeto {obj.name} no tiene un padre.");
            }
        }

        private IEnumerator RevertMaterialAfterDelay(Renderer renderer, float delay)
        {
            yield return new WaitForSeconds(delay);

            // Revertir al material predeterminado
            renderer.material = default_material;
            Debug.Log($"El material del objeto {renderer.gameObject.name} se ha revertido al material predeterminado.");
        }
    }
}