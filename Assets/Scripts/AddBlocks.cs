using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class AddBlocks : MonoBehaviour
{
    public List<Color> colors = new List<Color>();
    Color selectedColor;

    [SerializeField]
    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject playerPrefab;

    Camera arCamera;
    GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = null;
        Color customRed = new Color32(96, 28, 53, 1);
        Color customYellow = new Color32(255, 166, 48, 1);
        Color customDarkBlue = new Color32(46, 80, 118, 1);
        Color customLightBlue = new Color32(77, 162, 169, 1);
        colors.Add(customRed);
        colors.Add(customYellow);
        colors.Add(customDarkBlue);
        colors.Add(customLightBlue);
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

        //User assigned 1 of the 4 colors on startup
        selectedColor = colors[Random.Range(0, colors.Count - 1)];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        RaycastHit hit;
        Ray r = arCamera.ScreenPointToRay(Input.GetTouch(0).position);

        if (aRRaycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && playerObject == null)
            {
                if(Physics.Raycast(r, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawn")
                    {
                        playerObject = hit.collider.gameObject;
                    }
                    else
                    {
                        addPrefab(hits[0].pose.position);  
                        playerObject.GetComponent<MeshRenderer>().material.color = selectedColor;                  

                    }
                }
              
            }
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            playerObject = null;
        }
    }


     private void addPrefab(Vector3 blockPosition)
        {
            playerObject = Instantiate(playerPrefab, blockPosition, Quaternion.identity);
        }
    
}
