using TMPro;
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
        [SerializeField]
        private TMP_Text m_AlignmentText;

        [SerializeField]
        private TMP_Text m_ClassificationText;

        private TTSManager ttsManager;
        [SerializeField] private TTSModel model = TTSModel.TTS_1;
        [SerializeField] private TTSVoice voice = TTSVoice.Alloy;
        [SerializeField, Range(0.25f, 4.0f)] private float speed = 1f;

#if UNITY_INCLUDE_ARFOUNDATION
        private ARPlane m_Plane;

        void OnEnable()
        {
            m_Plane = GetComponent<ARPlane>();
            m_Plane.boundaryChanged += OnBoundaryChanged;

            // Automatically find TTSManager in the scene
            ttsManager = Object.FindFirstObjectByType<TTSManager>();
            if (ttsManager == null)
            {
                Debug.LogError("TTSManager not found in the scene. Make sure it exists and is active.");
            }
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
            if (ttsManager == null)
            {
                Debug.LogError("TTSManager is not assigned and could not be found.");
                return;
            }

            // Get classification and alignment as strings
            string classification = m_Plane.classifications.ToString();
            string alignment = m_Plane.alignment.ToString();

            // Update UI text
            m_ClassificationText.text = classification;
            m_AlignmentText.text = alignment;

            // Synthesize and play the text
            string textToSynthesize = $"This is a {classification} and it is {alignment} oriented.";
            Debug.Log(textToSynthesize);
            //ttsManager.SynthesizeAndPlay(textToSynthesize);

            // Update position
            transform.position = m_Plane.center;
        }
#endif
    }
}