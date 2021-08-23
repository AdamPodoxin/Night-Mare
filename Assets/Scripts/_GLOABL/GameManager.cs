using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject monologue;
    public GameObject preNightmare;

    private DataManager dataManager;

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();

        bool hasReadPrompts = dataManager.GetProgress().hasReadPrompts;
        if (hasReadPrompts)
        {
            monologue.SetActive(false);
            preNightmare.SetActive(true);
        }
    }
}
