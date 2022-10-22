using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotScript : MonoBehaviour
{
    public void onButtonClick()
    {
        //ScreenCapture.CaptureScreenshot("Screenshot:"+System.DateTime.Now+".png");
        ScreenCapture.CaptureScreenshot("Screenshot.png");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
