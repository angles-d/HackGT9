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
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager ar;
    public bool firstImageScanned = false; //tracks if first image was scanned
    public bool vidDone = false; //tracks if first image was scanned
    public GameObject finalVid = null;
    public VideoPlayer finalVidPlayer;

    public void Awake()
    {
        ar = FindObjectOfType<ARTrackedImageManager>();
        foreach (GameObject prefab in placeablePrefabs)
        {
            //instantiates a physical version of the prefab
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            newPrefab.SetActive(false);
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
        print("helllooooo");

        foreach (KeyValuePair<string, GameObject> kvp in spawnedPrefabs)
        {
            print("ooooo");
            print("Key, Value" +  kvp.Key + ":"+ kvp.Value);
        }

        finalVid = spawnedPrefabs["Activist"];
        finalVidPlayer = finalVid.GetComponentInChildren<VideoPlayer>();
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
            //tracks that the first image was scanned for the vignette controller
            if (!firstImageScanned) { firstImageScanned = true; }
            SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.updated)
        {
           SetImage(tracked);
        }

        foreach (ARTrackedImage tracked in args.removed)
        {
            spawnedPrefabs[tracked.name].SetActive(false);
        }

    }

    private void Update()
    {
        if (finalVid != null && !vidDone && finalVid.activeSelf)
        {
            checkVidsEnd();
        }
    }

    private void SetImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;


        GameObject newPrefab = spawnedPrefabs[name];


        newPrefab.transform.position = position;
        newPrefab.transform.rotation = rotation;

        newPrefab.SetActive(true);
  
        foreach (GameObject pref in spawnedPrefabs.Values)
        {
            if (pref.name != name)
            {
                pref.SetActive(false);
            }
        }

    }

    //check if the user is done playing the video

    void checkVidsEnd()
    {
        //Debug.Log("frame" +   finalVidPlayer.frame);
        if (finalVidPlayer.frame >= 1400)
        {
            Debug.Log("vid done");
            vidDone = true;
        }
    }

    




}
