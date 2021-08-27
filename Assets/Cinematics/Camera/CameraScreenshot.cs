using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraScreenshot : MonoBehaviour
{
    public KeyCode screenshotKey = KeyCode.F2;

    private void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        string path = Application.persistentDataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        print("Screenshot " + path + " taken");

        ScreenCapture.CaptureScreenshot(path);
    }
}
