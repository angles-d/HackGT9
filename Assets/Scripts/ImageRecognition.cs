using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.Video;

public class ImageRecognition : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private GameObject[] spawnedPrefabs = new GameObject[1];
    private ARTrackedImageManager ar;

    public void Awake()
    {
        ar = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in placeablePrefabs)
        {
            //instantiates a physical version of the prefab
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnedPrefabs[0] = newPrefab;
        }
       

     
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
            print("helllooooooo");
            SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.updated)
        {
           SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.removed)
        {
            spawnedPrefabs[0].SetActive(false);
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


        GameObject newPrefab = spawnedPrefabs[0];


        newPrefab.transform.position = position;
        newPrefab.transform.rotation = rotation;

        newPrefab.SetActive(true);
  
        //foreach (GameObject pref in spawnedPrefabs.Values)
        //{
        //    if (pref.name != name)
        //    {
        //        pref.SetActive(false);
        //    }
        //}

    }

}
