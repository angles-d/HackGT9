using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab;
        public GameObject gameBoard;
        public float boardWidth;

        public List<Color> colors = new List<Color>();
        Color selectedColor;



        //board boarders
        float left, right, top, bottom; 

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public void RemoveAllAnchors()
        {
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
        }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();

            Vector3 boardPos = gameBoard.transform.position;
            float halfWidth = boardWidth / 2.0f;
            left = boardPos.x - halfWidth;
            right = boardPos.x + halfWidth;
            top = boardPos.z + halfWidth;
            bottom = boardPos.z - halfWidth;


            Color customRed = new Color32(96, 28, 53, 1);
            Color customYellow = new Color32(255, 166, 48, 1);
            Color customDarkBlue = new Color32(46, 80, 118, 1);
            Color customLightBlue = new Color32(77, 162, 169, 1);
            colors.Add(customRed);
            colors.Add(customYellow);
            colors.Add(customDarkBlue);
            colors.Add(customLightBlue);
            //arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

            //User assigned 1 of the 4 colors on startup
            selectedColor = colors[Random.Range(0, colors.Count)];
           
        }
        private void Start()
        {
            //m_Prefab.GetComponent<MeshRenderer>().material.color = selectedColor;
            Logger.Log("colo: " + m_Prefab.GetComponent<MeshRenderer>().material.color);
        }

        void SetAnchorText(ARAnchor anchor, string text)
        {
            var canvasTextManager = anchor.GetComponent<CanvasTextManager>();
            if (canvasTextManager)
            {
                canvasTextManager.text = text;
            }
        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
           
            ARAnchor anchor = null;
            Vector3 hitPos = hit.pose.position;

            if (hitPos.x > right || hitPos.x < left || hitPos.z < top || hitPos.z > bottom) {
                // Otherwise, just create a regular anchor at the hit pose
                Logger.Log("Creating regular anchor.");

                // Note: the anchor can be anywhere in the scene hierarchy
                var gameObject = Instantiate(prefab, hitPos + new Vector3(0, 0.2f, 0), hit.pose.rotation);
                //gameObject.GetComponent<MeshRenderer>().material.color = selectedColor;

                // Make sure the new GameObject has an ARAnchor component
                anchor = gameObject.GetComponent<ARAnchor>();
                if (anchor == null)
                {
                    anchor = gameObject.AddComponent<ARAnchor>();
                }

                SetAnchorText(anchor, $"Anchor (from {hit.hitType})");
            }

            return anchor;
        }

      

        void Update()
        {
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;

            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                
           
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                }
                else
                {
                    Logger.Log("Block out of bounds: " + hit.pose.position);
                }
         
            }

           

           
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
