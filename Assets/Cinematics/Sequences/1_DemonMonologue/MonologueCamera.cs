using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueCamera : MonoBehaviour
{
    public GameObject monologue;
    public GameObject preNightmare;

    public void StartGame()
    {
        preNightmare.SetActive(true);
        monologue.SetActive(false);
    }
}
