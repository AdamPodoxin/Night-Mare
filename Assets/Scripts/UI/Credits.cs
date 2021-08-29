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
        CreateCredits();
    }

    private void Update()
    {
        ScrollCredits();

        if (Input.anyKeyDown && !_isExiting)
        {
            _isExiting = true;
            SceneManager.LoadSceneAsync("0_Menu", LoadSceneMode.Single);
        }
    }

    private void ScrollCredits()
    {
        creditsParent.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
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
                //newLine = "<b>" + line.Replace("--", "") + "</b>";

                centerText.text += "<b>" + line.Replace("--", "") + "</b>";
                //leftText.text += '\n';
                //rightText.text += '\n';
            }
            else if (line.Contains(" : "))
            {
                string[] splitLine = line.Split(':');

                leftText.text += splitLine[0];

                int websiteIndex = splitLine[1].IndexOf('(');
                rightText.text += splitLine[1].Substring(0, websiteIndex);

                //centerText.text += '\n';
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
                    //leftText.text += '\n';
                    //rightText.text += '\n';
                }
            }

            centerText.text += '\n';
            leftText.text += '\n';
            rightText.text += '\n';
        }
    }
}
