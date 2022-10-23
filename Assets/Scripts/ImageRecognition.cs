using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.XR.ARFoundation.Samples;
using UnityEngine.Video;

public class ImageRecognition : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private ARTrackedImageManager ar;
    private ARPlaneManager plane;
    public AnchorCreator anchor;

    public void Awake()
    {
        ar = FindObjectOfType<ARTrackedImageManager>();
        plane = GetComponent<ARPlaneManager>();
        anchor = GetComponent<AnchorCreator>();
        prefab.SetActive(false);
        anchor.enabled = false;
        plane.enabled = false;

    }

    public void OnEnable()
    {
        ar.trackedImagesChanged += onImageChanged;
    }

    public void OnDisable()
    {
        ar.trackedImagesChanged -= onImageChanged;
    }

    public void onImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage tracked in args.added)
        {
            SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.updated)
        {
           SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.removed)
        {
            prefab.SetActive(false);
        }

    }

    private void Update()
    {
    }

    private void SetImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;


        //GameObject newPrefab = prefab;


        prefab.transform.position = position;
        prefab.transform.rotation = rotation;

        prefab.SetActive(true);
        plane.enabled = true;
        anchor.enabled = true;
  

    }

}
