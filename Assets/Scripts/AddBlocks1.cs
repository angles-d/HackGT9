using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class AddBlocks1 : MonoBehaviour
{

    public List<Color> colors = new List<Color>();
   

    [SerializeField]
    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject playerPrefab;

    Camera arCamera;
    GameObject playerObject;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = null;
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

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

                    }
                }
              
            }
        }
        else if (Input.GetTouch(0).phase == TouchPhase.Moved && playerObject != null)
        {
            playerObject.transform.position = hits[0].pose.position;
            playerObject.GetComponent<Renderer>().material.color = colors[Random.Range(0, colors.Count - 1)];
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
