using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public TextAsset creditsAsset;

    [Space]

    public Text centerText;
    public Text leftText;
    public Text rightText;

    [Space]

    public RectTransform creditsParent;
    public float scrollSpeed = 0.5f;

    private bool _isExiting = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CreateCredits();
    }

    private void FixedUpdate()
    {
        ScrollCredits();

        if (Input.anyKeyDown) Exit();
    }

    private void ScrollCredits()
    {
        creditsParent.Translate(scrollSpeed * Time.fixedDeltaTime * Vector3.up);
    }

    public void CreateCredits()
    {
        centerText.text = "";
        leftText.text = "";
        rightText.text = "";

        string[] lines = creditsAsset.text.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("--"))
            {
                centerText.text += "<b>" + line.Replace("--", "") + "</b>";
            }
            else if (line.Contains(" : "))
            {
                string[] splitLine = line.Split(':');

                leftText.text += splitLine[0];

                if (line.Contains("("))
                {
                    int websiteIndex = splitLine[1].IndexOf('(');
                    rightText.text += splitLine[1].Substring(0, websiteIndex);
                }
                else
                {
                    rightText.text += splitLine[1];
                }
            }
            else
            {
                if (line.Contains("("))
                {
                    int websiteIndex = line.IndexOf('(');
                    centerText.text += line.Substring(0, websiteIndex);
                }
                else
                {
                    centerText.text += line;
                }
            }

            centerText.text += '\n';
            leftText.text += '\n';
            rightText.text += '\n';
        }
    }

    public void Exit()
    {
        if (!_isExiting)
        {
            _isExiting = true;
            SceneManager.LoadSceneAsync("0_Menu", LoadSceneMode.Single);
        }
    }
}
