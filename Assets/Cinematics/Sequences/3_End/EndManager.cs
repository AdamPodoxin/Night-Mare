using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public string choice;

    [Space]

    public Animator blueFlash;
    public Animator greenFlash;

    [Space]

    public GameObject choiceScene;
    public GameObject houseScene;

    public void MakeChoice(string choice)
    {
        this.choice = choice;

        switch (choice)
        {
            case "L":
                blueFlash.gameObject.SetActive(true);
                blueFlash.Play("Flash_Out");
                break;
            case "R":
                greenFlash.gameObject.SetActive(true);
                greenFlash.Play("Flash_Out");
                break;
        }

        choiceScene.SetActive(false);
        houseScene.SetActive(true);
    }
}
