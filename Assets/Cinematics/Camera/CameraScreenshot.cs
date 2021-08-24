using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraScreenshot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Print))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        string path = Application.persistentDataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        print(path);

        ScreenCapture.CaptureScreenshot(path);
    }
}
