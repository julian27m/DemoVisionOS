using UnityEngine;

#if UNITY_INCLUDE_ARFOUNDATION
using UnityEngine.XR.ARFoundation;
#endif

namespace PolySpatial.Samples
{
#if UNITY_INCLUDE_ARFOUNDATION
    [RequireComponent(typeof(ARPlane))]
#endif
    public class PlaneDataUI : MonoBehaviour
    {
        private string classification;
        private string alignment;

#if UNITY_INCLUDE_ARFOUNDATION
        private ARPlane m_Plane;

        void OnEnable()
        {
            m_Plane = GetComponent<ARPlane>();
            m_Plane.boundaryChanged += OnBoundaryChanged;
        }

        void OnDisable()
        {
            if (m_Plane != null)
            {
                m_Plane.boundaryChanged -= OnBoundaryChanged;
            }
        }

        void OnBoundaryChanged(ARPlaneBoundaryChangedEventArgs eventArgs)
        {
            if (m_Plane == null) return;

            // Actualiza clasificación y alineación del plano
            classification = m_Plane.classifications.ToString();
            alignment = m_Plane.alignment.ToString();
        }
#endif

        // Propiedades públicas para acceder a la clasificación y alineación
        public string Classification => classification;
        public string Alignment => alignment;
    }
}
