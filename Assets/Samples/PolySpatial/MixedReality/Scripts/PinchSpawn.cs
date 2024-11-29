using Unity.PolySpatial.InputDevices;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

#if UNITY_INCLUDE_XR_HANDS
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
#endif

namespace PolySpatial.Samples
{
    public class PinchSpawn : MonoBehaviour
    {
        [SerializeField]
        Transform m_InputAxisTransform; // Objeto visual para reflejar el input visual

        [SerializeField]
        LayerMask planeLayerMask; // Máscara para detectar los planos

#if UNITY_INCLUDE_XR_HANDS
        XRHandSubsystem m_HandSubsystem;
        XRHandJoint m_LeftIndexTipJoint;
        XRHandJoint m_LeftThumbTipJoint;
        Vector3 m_LeftMidPoint;
        bool m_ActiveLeftPinch;
        float m_ScaledThreshold;

        const float k_PinchThreshold = 0.02f;

        void Start()
        {
            EnhancedTouchSupport.Enable(); // Habilita el soporte táctil mejorado
            m_ScaledThreshold = k_PinchThreshold / transform.localScale.x; // Escala relativa
        }

        void Update()
        {
            if (!TryEnsureInitialized())
                return;

            // Actualiza las posiciones de las manos
            var updateSuccessFlags = m_HandSubsystem.TryUpdateHands(XRHandSubsystem.UpdateType.Dynamic);

            if ((updateSuccessFlags & XRHandSubsystem.UpdateSuccessFlags.LeftHandRootPose) != 0)
            {
                m_LeftIndexTipJoint = m_HandSubsystem.leftHand.GetJoint(XRHandJointID.IndexTip);
                m_LeftThumbTipJoint = m_HandSubsystem.leftHand.GetJoint(XRHandJointID.ThumbTip);

                if (DetectPinch(m_LeftIndexTipJoint, m_LeftThumbTipJoint, ref m_LeftMidPoint, ref m_ActiveLeftPinch))
                {
                    Debug.Log("Pinch detectado con la mano izquierda.");
                    HandleVisualInput();
                }
            }
        }

        void HandleVisualInput()
        {
            var activeTouches = Touch.activeTouches;

            if (activeTouches.Count > 0)
            {
                var primaryTouchData = EnhancedSpatialPointerSupport.GetPointerState(activeTouches[0]);

                // Actualiza la posición y rotación del TransformHandle
                m_InputAxisTransform.SetPositionAndRotation(
                    primaryTouchData.interactionPosition,
                    primaryTouchData.inputDeviceRotation
                );

                Debug.Log($"TransformHandle actualizado a posición: {m_InputAxisTransform.position}, rotación: {m_InputAxisTransform.rotation}");

                // Verifica si el input está sobre un plano
                Ray ray = new Ray(m_InputAxisTransform.position, m_InputAxisTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f, planeLayerMask))
                {
                    var hitObject = hit.collider.gameObject;
                    Debug.Log($"Raycast hit object: {hitObject.name}");

                    if (hitObject.TryGetComponent(out CustomPlaneVisualizer customVisualizer))
                    {
                        Debug.Log($"Plano detectado: {hitObject.name}");
                        customVisualizer.SetMeshVisibility(true); // Muestra la malla del plano
                    }
                    else
                    {
                        Debug.LogWarning("El objeto golpeado no tiene CustomPlaneVisualizer.");
                    }
                }
                else
                {
                    Debug.Log("No se detectó ningún plano en la dirección del TransformHandle.");
                }
            }
        }

        bool DetectPinch(XRHandJoint index, XRHandJoint thumb, ref Vector3 midPoint, ref bool activeFlag)
        {
            if (index.trackingState == XRHandJointTrackingState.None || thumb.trackingState == XRHandJointTrackingState.None)
            {
                Debug.LogWarning("Las posiciones del índice o pulgar no están siendo rastreadas.");
                return false;
            }

            Vector3 indexPOS = Vector3.zero;
            Vector3 thumbPOS = Vector3.zero;

            if (index.TryGetPose(out Pose indexPose))
            {
                indexPOS = indexPose.position;
            }

            if (thumb.TryGetPose(out Pose thumbPose))
            {
                thumbPOS = thumbPose.position;
            }

            var pinchDistance = Vector3.Distance(indexPOS, thumbPOS);

            if (pinchDistance <= m_ScaledThreshold)
            {
                if (!activeFlag)
                {
                    activeFlag = true;
                    midPoint = (indexPOS + thumbPOS) / 2;
                    return true;
                }
            }
            else
            {
                activeFlag = false;
            }

            return false;
        }

        bool TryEnsureInitialized()
        {
            if (m_HandSubsystem != null)
                return true;

            m_HandSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();
            return m_HandSubsystem != null;
        }
#endif
    }
}
