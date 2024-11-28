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

        private static int planeCounter = 0; // Static counter to track unique IDs
        private const int maxRecognitions = 10; // Limit for console logs

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
            // Stop logging if the counter exceeds the maximum limit
            if (planeCounter >= maxRecognitions)
            {
                return;
            }

            // Increment the counter and assign a unique ID
            planeCounter++;
            int planeId = planeCounter;

            // Get classification and alignment as strings
            string classification = m_Plane.classifications.ToString();
            string alignment = m_Plane.alignment.ToString();

            // Update UI text
            m_ClassificationText.text = classification;
            m_AlignmentText.text = alignment;

            // Log the information with the unique ID
            string logMessage = $"{planeId}: This is a {classification} and it is {alignment} oriented.";
            Debug.Log(logMessage);
            //ttsManager.SynthesizeAndPlay(textToSynthesize);

            // Update position
            transform.position = m_Plane.center;
        }
#endif
    }
}
